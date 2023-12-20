using Newtonsoft.Json;
using Simple.Rest.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.DataFile;
using VendingMachine.Models;
using VendingMachine.Models.Request;
using VendingMachine.Models.Response;
using VendingMachine.Models.Response.Remote;
using VendingMachine.Utils;

namespace VendingMachine.Services
{
    public class ApiService : IApiService
    {
        private readonly IRestClient _restClient;
        private readonly ISchedulerService _schedulerService;
        private readonly LocalDBContext _dbContext;

        private Token _token = null;
        private MerchantDevice _device = null;
        private Task lastAction = null;
        private bool _isRefreshing = false;

        public event IApiService.OnError OnErrorOccur;

        public ApiService(
            LocalDBContext localDBContext,
            ISchedulerService schedulerService,
            IRestClient restClient)
        {
            _dbContext = localDBContext;
            _schedulerService = schedulerService;
            _restClient = restClient;
        }

        public MerchantDevice getDevice()
        {
            if (_device != null)
            {
                return _device;
            }
            using (var dataContext = new LocalDBContext())
            {
                _device = dataContext.Devices.FirstOrDefault();

                return _device;
            }
        }


        public Token getToken()
        {
            if (_token != null)
            {
                return _token;
            }

            if (_dbContext != null)
            {
                _token = _dbContext.Tokens.FirstOrDefault();
            }

            return _token;
        }


        public IObservable<PagingResponse<RemoteProduct>> GetAllProduct()
        {
            if (getDevice() == null)
            {
                return Observable.Empty<PagingResponse<RemoteProduct>>();
            }

            Debug.WriteLine("Pro401_1" + _restClient.Headers["Authorization"]);
            var request = new GetProductsRequest()
            {
                DeviceId = _device.Id,
                Keyword = "",
                ProductTypeId = "",
                Length = 100,
                Start = 0
            };


            return _restClient.PostAsync<GetProductsRequest, BaseResponse<PagingResponse<RemoteProduct>>>(Constants.Server.MainProduct, request)
                .ToObservable().Take(1).Select(x =>
                {
                    Debug.WriteLine(JsonConvert.SerializeObject(x.Resource.Result.Data));

                    return x.Resource.Result.Data;
                })
                .Catch<PagingResponse<RemoteProduct>, Exception>(exception =>
                {
                    return HandleException(exception, GetAllProduct());
                });

        }

        public IObservable<LoginResponse> Login(string userName, string password, string deviceCode)
        {
            var request = new GetTokenRequest()
            {
                UserName = userName,
                PassWord = password,
                Provider = "D",
                DeviceCode = deviceCode
            };

            return _restClient.PostAsync<GetTokenRequest, BaseResponse<LoginResponse>>(Constants.Server.Login, request)
                .ToObservable().Take(1).Select(x =>
                {
                    Debug.WriteLine(JsonConvert.SerializeObject(x.Resource.Result.Data));

                    return x.Resource.Result.Data;
                })
                .Catch<LoginResponse, Exception>(exception =>
                {
                    return HandleException(exception, Login(userName, password, deviceCode));
                });

        }

        public IObservable<List<RemoteType>> GetAllType()
        {
            return _restClient.GetAsync<BaseResponse<List<RemoteType>>>(Constants.Server.MainType)
               .ToObservable().Take(1).Select(x =>
               {
                   Debug.WriteLine(JsonConvert.SerializeObject(x.Resource.Result.Data));
                   return x.Resource.Result.Data;
               })
               .Catch<List<RemoteType>, Exception>(exception =>
               {
                   return HandleException(exception, GetAllType());
               });
        }

        public void GetToken()
        {
            Debug.WriteLine("Gettoken");
            var request = new GetTokenRequest()
            {
                UserName = "admin",
                PassWord = "123456X"
            };
            var tokenTask = _restClient.PostAsync<GetTokenRequest, BaseResponse<RemoteToken>>(Constants.Server.Login, request);
            tokenTask.Wait();
            _token = new Token()
            {
                UserId = tokenTask.Result.Resource.Result.Data.UserId,
                AccessToken = tokenTask.Result.Resource.Result.Data.AccessToken,
                RefreshToken = tokenTask.Result.Resource.Result.Data.RefreshToken
            };
            Debug.WriteLine(JsonConvert.SerializeObject(_token));
            _restClient.Headers["Authorization"] = _token.AccessToken;
        }

        private IObservable<T> HandleException<T>(Exception exception, IObservable<T> retry)
        {
            if (exception.GetType().Equals(typeof(WebException)))
            {
                var webException = exception as WebException;
                if (webException.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = webException.Response as HttpWebResponse;
                    if (response != null && response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                    {
                        Thread.Sleep(500);
                        Debug.WriteLine(_isRefreshing);
                        if (_isRefreshing)
                        {
                            while (_isRefreshing)
                            {
                                Debug.WriteLine("sleep");
                                Thread.Sleep(100);
                            }
                            Debug.WriteLine("retry");
                            return retry;
                        }
                        else
                        {
                            _isRefreshing = true;
                            GetToken();
                            _isRefreshing = false;
                            Debug.WriteLine("retry");
                            return retry;
                        }


                    }
                    else
                    {
                        OnErrorOccur(0, exception.Message);
                        return Observable.Empty<T>();
                    }
                }
                else
                {
                    OnErrorOccur(0, exception.Message);
                    return Observable.Empty<T>();
                }


            }
            OnErrorOccur(0, exception.Message);
            return Observable.Empty<T>();

        }

        public void setToken()
        {
            if (_restClient.Headers["Authorization"] == null)
            {
                _restClient.Headers.Add("Authorization", _token.AccessToken);
            }
            else
            {
                _restClient.Headers["Authorization"] = _token.AccessToken;
            }
        }

        public IObservable<List<PaymentMethod>> GetPaymentMethod()
        {

            return _restClient.GetAsync<BaseResponse<List<PaymentMethod>>>(Constants.Server.GetPaymentMethods)
               .ToObservable().Take(1).Select(x =>
               {
                   Debug.WriteLine(JsonConvert.SerializeObject(x.Resource.Result.Data));
                   return x.Resource.Result.Data;
               })
               .Catch<List<PaymentMethod>, Exception>(exception =>
               {
                   return HandleException(exception, GetPaymentMethod());
               });
        }

        public IObservable<Order> CreateOrder(List<ProductDetail> orderDetails, PaymentMethod paymentMethod)
        {
            Debug.WriteLine("ProType401_1" + _restClient.Headers["Authorization"]);

            if (getDevice() == null)
            {
                return Observable.Empty<Order>();
            }

            if (getToken() == null)
            {
                return Observable.Empty<Order>();
            }

            setToken();

            var request = new CreateOrderRequest()
            {
                CustomerId = _token.UserId.ToString(),
                DeviceId = _device.Id,
                PaymentTypeId = paymentMethod.Id,
                OrderDetails = orderDetails,
                CurrencyCode = "VND",
                StoreId = _device.MerchantStoreId                
            };

            Debug.WriteLine("REQUEST: " + JsonConvert.SerializeObject(request));

            return _restClient.PostAsync<CreateOrderRequest, BaseResponse<Order>>(Constants.Server.CreateOrder, request)
               .ToObservable().Take(1).Select(x =>
               {
                   Debug.WriteLine(JsonConvert.SerializeObject(x.Resource.Result.Data));
                   return x.Resource.Result.Data;
               })
               .Catch<Order, Exception>(exception =>
               {
                   return HandleException(exception, CreateOrder(orderDetails, paymentMethod));
               });
        }

        public IObservable<Order> GetOrderDetail(string orderId, string transactionCode)
        {

            if (getDevice() == null)
            {
                return Observable.Empty<Order>();
            }

            if (getToken() == null)
            {
                return Observable.Empty<Order>();
            }

            setToken();

            Debug.WriteLine("GetOrderDetail" + _restClient.Headers["Authorization"]);

            var request = new CheckPaymentRequest()
            {
                OrderId = orderId,
                TransactionCode = transactionCode
            };
            Debug.WriteLine("REQUEST: " + JsonConvert.SerializeObject(request));



            return _restClient.PostAsync<CheckPaymentRequest, BaseResponse<Order>>(Constants.Server.CheckPayment, request)
               .ToObservable().Take(1).Select(x =>
               {
                   Debug.WriteLine(JsonConvert.SerializeObject(x.Resource.Result.Data));
                   return x.Resource.Result.Data;
               })
               .Catch<Order, Exception>(exception =>
               {
                   return HandleException(exception, GetOrderDetail(orderId, transactionCode));
               });
        }

        public IObservable<Order> UpdateOrderResult(string orderId, string transactionCode, List<ProductResult> productsResult)
        {
            Debug.WriteLine("UpdateOrderResult" + _restClient.Headers["Authorization"]);
            if (getDevice() == null)
            {
                return Observable.Empty<Order>();
            }

            if (getToken() == null)
            {
                return Observable.Empty<Order>();
            }

            setToken();

            var request = new UpdateResultRequest()
            {
                OrderId = orderId,
                TransactionCode = transactionCode,
                Products = productsResult
            };

            Debug.WriteLine("REQUEST: " + JsonConvert.SerializeObject(request));

              return _restClient.PostAsync<UpdateResultRequest, BaseResponse<Order>>(Constants.Server.UpdateOrderResult, request)
               .ToObservable().Take(1).Select(x =>
               {
                   Debug.WriteLine(JsonConvert.SerializeObject(x.Resource.Result.Data));
                   return x.Resource.Result.Data;
               })
               .Catch<Order, Exception>(exception =>
               {
                   return HandleException(exception, UpdateOrderResult(orderId,transactionCode,productsResult ));
               });
        }
    }
}
