using Simple.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Models.Request;
using VendingMachine.Models.Response;
using VendingMachine.Models.Response.Remote;

namespace VendingMachine.Services
{
    public interface IApiService
    {

        public delegate void OnError(int errorCode, string message);  // delegate

        public event OnError OnErrorOccur;
        public void GetToken();
        public IObservable<PagingResponse<RemoteProduct>> GetAllProduct();

        public IObservable<Order> CreateOrder(List<ProductDetail> orderDetails, PaymentMethod paymentMethod);
        public IObservable<Order> GetOrderDetail(string orderId, string transactionCode);

        public IObservable<Order> UpdateOrderResult(string orderId, string transactionCode, List<ProductResult> productsResult);

        public IObservable<List<PaymentMethod>> GetPaymentMethod();

        public IObservable<List<RemoteType>> GetAllType();

        public IObservable<LoginResponse> Login(string userName, string password, string deviceCode);

    }
}
