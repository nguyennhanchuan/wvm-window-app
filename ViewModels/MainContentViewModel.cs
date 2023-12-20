using Newtonsoft.Json;
using Simple.Rest.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Windows.Data;
using System.Windows.Input;
using VendingMachine.Collections;
using VendingMachine.Commands;
using VendingMachine.DataFile;
using VendingMachine.Extensions;
using VendingMachine.Models;
using VendingMachine.Models.Request;
using VendingMachine.Models.Response;
using VendingMachine.Models.Response.Remote;
using VendingMachine.Navigation;
using VendingMachine.Services;

namespace VendingMachine.ViewModels
{
    public sealed class MainContentViewModel : BaseViewModel, IMainContentViewModel
    {
        private readonly ListCollectionView _collectionView;
        private readonly IApiService _apiService;
        private readonly IUartService _uartService;
        private readonly LocalDBContext _dBContext;

        private readonly ISchedulerService _schedulerService;

        private bool _isServerOnline;
        private string _serverStatus;

        private ObservableCollection<ProductType> _productTypes;
        private ObservableCollection<MerchantProduct> _products { get; set; }
        private ObservableCollection<PaymentMethod> _paymentMethods { get; set; }
        private ObservableCollection<MerchantProduct> _filteredProducts { get; set; }
        private ObservableCollection<CartItem> _refundProducts { get; set; }


        private Order _order;
        private Order _refundOrder;



        private PaymentMethod _selectedPaymentMethod = null;
        private System.Timers.Timer _orderCheckTimer = null;
        private System.Timers.Timer _resetTimer = null;
        private System.Timers.Timer _rePaymentTimer = null;



        private Cart _cart;
        private bool _cartVisible = false;
        private bool _errorVisible = false;
        private bool _isLoading = false;
        private bool _successVisible = false;
        private bool _failedVisible = false;
        private bool _QRVisible = false;
        private bool _paymentMethodVisible = false;
        private bool _refundResultVisible = false;
        private int _rePaymentLeft = 30;
        private string _loadingContent = "";
        private string _searchContent = "";
        private string _errorContent = "";

        public MainContentViewModel(
            LocalDBContext dBContext,
            IDiagnosticsViewModel diagnosticsViewModel,
            IApiService apiService,
            IUartService uartService,
            ISchedulerService schedulerService)
        {
            _dBContext = dBContext;
            _apiService = apiService;
            _schedulerService = schedulerService;
            _uartService = uartService;

            _paymentMethods = new ObservableCollection<PaymentMethod>();

            Diagnostics = diagnosticsViewModel;
            _productTypes = new ObservableCollection<ProductType>();
            _products = new ObservableCollection<MerchantProduct>();
            _refundProducts = new ObservableCollection<CartItem>();
            _filteredProducts = new ObservableCollection<MerchantProduct>();
            _cart = new Cart();

            NewSessionCommand = ReactiveCommand.Create()
                .DisposeWith(this);

            NewSessionCommand.ActivateGestures().Subscribe(x => StartNewSession());

            ToggleCartCommand = ReactiveCommand.Create()
              .DisposeWith(this);

            ToggleCartCommand.ActivateGestures().Subscribe(x => ToggleCart());


            AddProductCommand = ReactiveCommand.Create()
              .DisposeWith(this);

            AddProductCommand.Subscribe(x => AddProduct((Guid)x));

            RemoveProductCommand = ReactiveCommand.Create()
           .DisposeWith(this);

            RemoveProductCommand.Subscribe(x => RemoveProduct((Guid)x));

            FilterByTypeCommand = ReactiveCommand.Create()
            .DisposeWith(this);

            FilterByTypeCommand.Subscribe(x => FilterByType((ProductType)x));

            ProcessPaymentCommand = ReactiveCommand.Create()
            .DisposeWith(this);

            ProcessPaymentCommand.Subscribe(x => CreateOrder());

            SelectPaymentMedthodCommand = ReactiveCommand.Create()
         .DisposeWith(this);

            SelectPaymentMedthodCommand.Subscribe(x => SelectPaymentMedthod((PaymentMethod)x));

            TogglePaymentMethodCommand = ReactiveCommand.Create()
            .DisposeWith(this);

            TogglePaymentMethodCommand.Subscribe(x => TogglePaymentMethod());

            ClearDataCommand = ReactiveCommand.Create()
        .DisposeWith(this);

            ClearDataCommand.Subscribe(x => ClearData());

            ToggleQRCommand = ReactiveCommand.Create()
       .DisposeWith(this);

            ToggleQRCommand.Subscribe(x => ToggleQR());

            RePaymentCommand = ReactiveCommand.Create()
     .DisposeWith(this);

            RePaymentCommand.Subscribe(x =>
            {
                StopRePaymentTimer();
                FailedVisible = false;
                CreateOrder();
            });

            ToggleErrorCommand = ReactiveCommand.Create()
     .DisposeWith(this);

            ToggleErrorCommand.Subscribe(x =>
            {
                ToggleError();
            });

            FilterTextChangedCommand = ReactiveCommand.Create()
    .DisposeWith(this);

            FilterTextChangedCommand.Subscribe(x =>
            {
                FilterByText((string)x);
            });


            ClearSearchCommand = ReactiveCommand.Create()
    .DisposeWith(this);

            ClearSearchCommand.Subscribe(x =>
            {
                SearchContent = "";
                FilterByText(SearchContent);
            });

            _uartService.StartService();
            _uartService.OnFeedDone += new IUartService.Notify(OnFeedDone);

            _apiService.OnErrorOccur += new IApiService.OnError(OnErrorOccur);


            var _device = dBContext.Devices.FirstOrDefault();
            if (_device != null)
            {
                StartNewSession();
            }

        }

        private void FilterByText(string x)
        {
            var selectedProductTypes = _productTypes.Where(productType => productType.IsSelected);
            if (selectedProductTypes.Count() == 0)
            {
                FilteredProducts = _products.Where(product => product.Name.ToLower().Contains(x.ToLower())).ToObservableCollection();
            }
            else
            {
                FilteredProducts = _products.Where(product
                    => selectedProductTypes.Any(selectedProductType => product.ProductTypeId.Contains(selectedProductType.Id.ToString()))
                        && product.Name.ToLower().Contains(x.ToLower()))
                    .ToObservableCollection();
            }
        }

        private void OnErrorOccur(int errorCode, string message)
        {
            ErrorContent = message;
            ErrorVisible = true;
            StartResetTimer(3000);
        }

        private void ToggleError()
        {
            if (ErrorVisible)
            {
                ErrorContent = "";
                StartResetTimer(100);

            }
            ErrorVisible = !ErrorVisible;
        }

        private void OnFeedDone(List<FeedResult> feedResults)
        {
            Debug.WriteLine("ONFEEDDONE");
            var updateList = new List<ProductResult>();
            foreach (var orderDetail in _order.OrderDetails)
            {
                var productResult = new ProductResult();

                var results = feedResults.Where(result => result.command.productId == orderDetail.ProductId).ToList();
                foreach (var position in orderDetail.Positions)
                {
                    var success = results.Where(result => result.command.row == position.Row && result.command.col == position.Column && result.status == FeedResultStatus.SUCCESS).ToList();
                    productResult.Positions.Add(new ProductResultPosition()
                    {
                        Id = position.Id,
                        Column = position.Column,
                        Row = position.Row,
                        Quantity = success.Sum(result => result.command.number)
                    });
                }
                productResult.MerchantProductId = orderDetail.ProductId;
                updateList.Add(productResult);
            }

            SendUpdateResult(updateList);
        }

        private void SendUpdateResult(List<ProductResult> updateList)
        {
            Debug.WriteLine("UPDATERESULT");
            _apiService.UpdateOrderResult(_order.Id, _order.TransactionCode, updateList)
                .ObserveOn(_schedulerService.Dispatcher)
                .Subscribe((result) =>
                {
                    if (result.RefundAmount > 0)
                    {
                        SuccessVisible = false;
                        FailedVisible = false;
                        RefundProducts = updateList.Select(x =>
                        {
                            var product = _products.Where(product => product.Id.ToString() == x.MerchantProductId).FirstOrDefault();
                            var quantity = x.Positions.Sum(pos => pos.Quantity);
                            var orderDetail = result.OrderDetails.Where(detail => detail.ProductId.ToString() == x.MerchantProductId).FirstOrDefault();
                            if (product == null || orderDetail == null || orderDetail.Quantity == quantity)
                            {
                                return null;
                            }

                            return new CartItem()
                            {
                                Product = product,
                                Quantity = orderDetail.Quantity - quantity,
                                Total = (orderDetail.Quantity - quantity) * product.Price
                            };
                        }).Where(x => x != null).ToObservableCollection();

                        RefundOrder = result;
                        RefundResultVisible = true;

                        StartResetTimer(20000);
                    }
                    else
                    {
                        StartResetTimer(3000);

                    }
                })
                .DisposeWith(this);
        }

        private void ShowSuccess()
        {
            QRVisible = false;
            SuccessVisible = true;
            StartResetTimer(180000);
        }

        private void ShowLoading(string content)
        {
            IsLoading = true;
            LoadingContent = content;
        }

        private void HideLoading()
        {
            IsLoading = false;
            LoadingContent = "";
        }

        private void ToggleQR()
        {
            if (QRVisible)
            {
                StopOrderCheckTimer();
            }
            QRVisible = !_QRVisible;
        }

        private void ClearData()
        {
            _dBContext.Database.EnsureDeleted();
            Navigator.Navigate(Screens.LOGIN_LAYOUT);
        }

        private void CreateOrder()
        {
            ShowLoading("");
            StartResetTimer(300000);
            PaymentMethodVisible = false;
            _apiService.CreateOrder(_cart.CartItems.Select(x => new ProductDetail()
            {
                ProductId = x.Product.Id.ToString(),
                Quantity = x.Quantity
            }).ToList(), SelectedPaymentMethod)
               .ObserveOn(_schedulerService.Dispatcher)
               .Subscribe((order) =>
               {
                   HideLoading();
                   _order = order;
                   if (!string.IsNullOrEmpty(_order.QrCode))
                   {
                       QRVisible = true;
                       StartOrderCheckTimer();
                   }
                   OnPropertyChanged(() => Order);
               })
               .DisposeWith(this); ;
        }

        public void StartOrderCheckTimer()
        {
            _orderCheckTimer = new System.Timers.Timer();
            _orderCheckTimer.Elapsed += new ElapsedEventHandler(OnOrderCheckElapsed);
            _orderCheckTimer.Interval = 10000;
            _orderCheckTimer.Enabled = true;
            _orderCheckTimer.AutoReset = false;
            _orderCheckTimer.Start();
        }


        private void StopOrderCheckTimer()
        {
            if (_orderCheckTimer != null)
            {
                _orderCheckTimer.Enabled = false;
                _orderCheckTimer.Stop();
                _orderCheckTimer.Dispose();
            }
        }


        public void FeedProduct()
        {
            foreach (var orderDetail in _order.OrderDetails)
            {
                foreach (var position in orderDetail.Positions)
                {
                    var quantity = position.Quantity;
                    while (quantity > 0)
                    {
                        _uartService.Feed(orderDetail.ProductId, position.Id, 1, position.Row, position.Column);
                        quantity--;
                    }
                }
            }
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private void OnOrderCheckElapsed(object source, ElapsedEventArgs e)
        {
            if (_orderCheckTimer != null)
            {
                _orderCheckTimer.Enabled = false;
                _orderCheckTimer.Stop();
                _orderCheckTimer.Dispose();
            }

            _apiService.GetOrderDetail(_order.Id, _order.TransactionCode)
                .ObserveOn(_schedulerService.Dispatcher)
                .Subscribe((order) =>
                {
                    Debug.WriteLine(order.StatusCode);
                    //order.StatusCode = OmsOrderStatus.Paymented;
                    if (order.StatusCode == OmsOrderStatus.Paymented)
                    {
                        ShowSuccess();
                        FeedProduct();
                    }
                    else if (order.StatusCode == OmsOrderStatus.Cancelled)
                    {
                        ShowFailed();
                    }
                    else
                    {
                        StartOrderCheckTimer();
                    }
                })
                .DisposeWith(this);
        }

        private void ShowFailed()
        {
            QRVisible = false;
            FailedVisible = true;
            StartResetTimer(30000);
            StartRePaymentTimer();
        }

        private void StartRePaymentTimer()
        {
            if (_rePaymentTimer != null)
            {
                _rePaymentTimer.Enabled = false;
                _rePaymentTimer.Stop();
                _rePaymentTimer.Dispose();
            }

            _rePaymentTimer = new System.Timers.Timer();
            _rePaymentTimer.Elapsed += new ElapsedEventHandler(UpdateRepaymentLeft);
            _rePaymentTimer.Interval = 1000;
            _rePaymentTimer.Enabled = true;
            _rePaymentTimer.Start();
        }

        private void StopRePaymentTimer()
        {
            if (_rePaymentTimer != null)
            {
                _rePaymentTimer.Enabled = false;
                _rePaymentTimer.Stop();
                _rePaymentTimer.Dispose();
                RePaymentLeft = 30;
            }
        }

        private void UpdateRepaymentLeft(object sender, ElapsedEventArgs e)
        {
            if (_rePaymentLeft > 0)
            {
                RePaymentLeft = _rePaymentLeft - 1;
                StartRePaymentTimer();
            }
        }

        private void StartResetTimer(int delay)
        {
            if (_resetTimer != null)
            {
                _resetTimer.Enabled = false;
                _resetTimer.Stop();
                _resetTimer.Dispose();
            }

            _resetTimer = new System.Timers.Timer();
            _resetTimer.Elapsed += new ElapsedEventHandler(Reset);
            _resetTimer.Interval = delay;
            _resetTimer.Enabled = true;
            _resetTimer.Start();
        }

        private void Reset(object sender, ElapsedEventArgs e)
        {

            Debug.WriteLine("RESET");
            if (_orderCheckTimer != null)
            {
                _orderCheckTimer.Enabled = false;
                _orderCheckTimer.Stop();
                _orderCheckTimer.Dispose();
            }

            if (_rePaymentTimer != null)
            {
                _rePaymentTimer.Enabled = false;
                _rePaymentTimer.Stop();
                _rePaymentTimer.Dispose();
            }
            _rePaymentLeft = 30;
            Cart = new Cart();
            CartVisible = false;
            Order = null;
            RefundOrder = null;
            SelectedPaymentMethod = null;
            SuccessVisible = false;
            FailedVisible = false;
            QRVisible = false;
            PaymentMethodVisible = false;
            RefundResultVisible = false;
            StartNewSession();
        }

        private void SelectPaymentMedthod(PaymentMethod x)
        {
            SelectedPaymentMethod = x;
            foreach (var paymentMethod in _paymentMethods)
            {
                if (paymentMethod.Id == _selectedPaymentMethod.Id)
                {
                    paymentMethod.IsSelected = true;
                }
                else
                {
                    paymentMethod.IsSelected = false;
                }
            }
            OnPropertyChanged(() => PaymentMethods);

        }

        private void TogglePaymentMethod()
        {
            PaymentMethodVisible = !_paymentMethodVisible;

        }

        private void FilterByType(ProductType x)
        {
            var productType = _productTypes.Where(productType => productType.Id == x.Id).FirstOrDefault();
            if (productType != null)
            {
                productType.IsSelected = !x.IsSelected;
            }

            //OnPropertyChanged(() => ProductTypes);

            var selectedProductTypes = _productTypes.Where(productType => productType.IsSelected);
            if (selectedProductTypes.Count() == 0)
            {
                FilteredProducts = _products.ToObservableCollection();
            }
            else
            {
                FilteredProducts = _products.Where(product => selectedProductTypes.Any(selectedProductType => product.ProductTypeId.Contains(selectedProductType.Id.ToString()))).ToObservableCollection();
            }
        }

        private void RemoveProduct(Guid productId)
        {
            var product = _products.Where(product => product.Id == productId).FirstOrDefault();
            if (product != null)
            {
                _cart.RemoveProduct(product);
                product.AddStock(1);
                OnPropertyChanged(() => Cart);
                OnPropertyChanged(() => FilteredProducts);

            }
        }

        private void AddProduct(Guid productId)
        {
            var product = _products.Where(product => product.Id == productId).FirstOrDefault();
            if (product != null && product.Stock > 0)
            {
                _cart.AddProduct(product);
                product.RemoveStock(1);
                OnPropertyChanged(() => Cart);
                OnPropertyChanged(() => FilteredProducts);
            }
        }

        private void ToggleCart()
        {
            CartVisible = !_cartVisible;
        }

        private void StartNewSession()
        {
            if (_uartService.IsConnected())
            {
                Debug.WriteLine("CONNECTED");
            }

            //_uartService.LightOff();

            //Thread.Sleep(2000);

            //_uartService.LightOn();


            StartResetTimer(600000);

            var random = new Random();
            ShowLoading("");

            _apiService.GetAllProduct()
               .ObserveOn(_schedulerService.Dispatcher)
               .Subscribe((productResponse) =>
               {
                   HideLoading();
                   _products = productResponse.Data.Where(x => x.Quantity > 0).Select(x => new MerchantProduct()
                   {
                       Id = Guid.Parse(x.MerchantProductId),
                       ProductId = x.ProductId,
                       Name = x.Name,
                       Price = (double)x.Price,
                       Stock = x.Quantity,
                       ProductTypeId = x.ProductTypes.Select(y => y.MainTypeId.ToString()).ToList(),
                       Col = random.Next(1, 10),
                       ProductImage = "http://wvm.microteam.asia/" + x.ImageUrl,
                       Row = 1
                   }).ToObservableCollection();
                   FilteredProducts = _products.ToObservableCollection();
               })
               .DisposeWith(this);

            _apiService.GetAllType()
                .ObserveOn(_schedulerService.Dispatcher)
                .Subscribe((productTypeResponse) =>
                {
                    try
                    {
                        ProductTypes = productTypeResponse.Select(x => new ProductType()
                        {
                            Id = Guid.Parse(x.Id),
                            Name = x.Name,
                            ProductTypeImage = "http://wvm.microteam.asia/" + x.Thumbnail,
                        }).ToObservableCollection();

                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                })
                .DisposeWith(this);

            _apiService.GetPaymentMethod()
                 .ObserveOn(_schedulerService.Dispatcher)
                    .Subscribe((paymentTypeResponse) =>
                    {
                        try
                        {
                            PaymentMethods = paymentTypeResponse.Select(x => new PaymentMethod()
                            {
                                Id = x.Id,
                                Thumbnail = "http://wvm.microteam.asia/" + x.Thumbnail,
                                DisplayName = x.DisplayName,
                                Name = x.Name,
                                Order = x.Order
                            }).OrderBy(x => x.Order).ToObservableCollection();

                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    })
                    .DisposeWith(this);
        }

        public IEnumerable Metadata => _collectionView;


        public ObservableCollection<MerchantProduct> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged(() => FilteredProducts);
                OnPropertyChanged(() => HasProduct);
            }
        }

        public ObservableCollection<CartItem> RefundProducts
        {
            get
            {
                return _refundProducts;
            }
            set
            {
                _refundProducts = value;
                OnPropertyChanged(() => RefundProducts);
            }
        }

        public ObservableCollection<ProductType> ProductTypes
        {
            get
            {
                return _productTypes;
            }
            set
            {
                _productTypes = value;
                OnPropertyChanged(() => ProductTypes);
            }
        }

        public ObservableCollection<PaymentMethod> PaymentMethods
        {
            get => _paymentMethods.ToObservableCollection();
            set
            {
                _paymentMethods = value;
                OnPropertyChanged(() => PaymentMethods);
            }
        }

        public int RePaymentLeft
        {
            get => _rePaymentLeft;
            private set
            {
                _rePaymentLeft = value;
                OnPropertyChanged(() => RePaymentLeft);
            }
        }

        public bool QRVisible
        {
            get => _QRVisible;
            private set
            {
                _QRVisible = value;
                OnPropertyChanged(() => QRVisible);
            }
        }

        public bool ErrorVisible
        {
            get => _errorVisible;
            private set
            {
                _errorVisible = value;
                OnPropertyChanged(() => ErrorVisible);
            }
        }

        public Order Order
        {
            get => _order;
            private set
            {
                _order = value;
                OnPropertyChanged(() => Order);
            }
        }

        public Order RefundOrder
        {
            get => _refundOrder;
            private set
            {
                _refundOrder = value;
                OnPropertyChanged(() => RefundOrder);
            }
        }

        public bool HasProduct
        {
            get => _filteredProducts.Count > 0;
        }

        public bool CartVisible
        {
            get => _cartVisible;
            private set
            {
                _cartVisible = value;
                OnPropertyChanged(() => CartVisible);
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;
                OnPropertyChanged(() => IsLoading);
            }
        }

        public string LoadingContent
        {
            get => _loadingContent;
            private set
            {
                _loadingContent = value;
                OnPropertyChanged(() => LoadingContent);
            }
        }

        public string SearchContent
        {
            get => _searchContent;
            set
            {
                _searchContent = value;
                OnPropertyChanged(() => SearchContent);
            }
        }

        public string ErrorContent
        {
            get => _errorContent;
            private set
            {
                _errorContent = value;
                OnPropertyChanged(() => ErrorContent);
            }
        }

        public bool SuccessVisible
        {
            get => _successVisible;
            private set
            {
                _successVisible = value;
                OnPropertyChanged(() => SuccessVisible);
            }
        }


        public bool FailedVisible
        {
            get => _failedVisible;
            private set
            {
                _failedVisible = value;
                OnPropertyChanged(() => FailedVisible);
            }
        }


        public bool PaymentMethodVisible
        {
            get => _paymentMethodVisible;
            private set
            {
                _paymentMethodVisible = value;
                OnPropertyChanged(() => PaymentMethodVisible);
            }
        }

        public bool RefundResultVisible
        {
            get => _refundResultVisible;
            private set
            {
                _refundResultVisible = value;
                OnPropertyChanged(() => RefundResultVisible);
            }
        }

        public PaymentMethod SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            private set
            {
                _selectedPaymentMethod = value;
                OnPropertyChanged(() => SelectedPaymentMethod);
            }
        }



        public bool IsServerOnline
        {
            get => _isServerOnline;
            private set
            {
                _isServerOnline = value;
                OnPropertyChanged(() => IsServerOnline);
            }
        }

        public Cart Cart
        {
            get => _cart;
            private set
            {
                _cart = value;
                OnPropertyChanged(() => Cart);
            }
        }

        public string ServerStatus
        {
            get => _serverStatus;
            private set { SetPropertyAndNotify(ref _serverStatus, value, () => ServerStatus); }
        }

        public ReactiveCommand<object> RefreshCommand { get; }

        public ReactiveCommand<object> AddProductCommand { get; }

        public ReactiveCommand<object> RemoveProductCommand { get; }

        public ReactiveCommand<object> FilterByTypeCommand { get; }

        public ReactiveCommand<object> ToggleCartCommand { get; }

        public ReactiveCommand<object> ProcessPaymentCommand { get; }

        public ReactiveCommand<object> SelectPaymentMedthodCommand { get; }

        public ReactiveCommand<object> NewSessionCommand { get; }

        public ReactiveCommand<object> TogglePaymentMethodCommand { get; }

        public ReactiveCommand<object> ToggleQRCommand { get; }

        public ReactiveCommand<object> ToggleErrorCommand { get; }

        public ReactiveCommand<object> ClearDataCommand { get; }

        public ReactiveCommand<object> RePaymentCommand { get; }

        public ReactiveCommand<object> FilterTextChangedCommand { get; }
        public ReactiveCommand<object> ClearSearchCommand { get; }

        public IDiagnosticsViewModel Diagnostics { get; }


        private IObservable<Unit> ObserveRefresh()
        {
            return RefreshCommand.AsUnit();
        }

        // Show Menu
        public void ShowMenu()
        {
            IsPanelVisible = true;
        }
        // Show Menu Command
        private ICommand _showMenuCommand;
        public ICommand ShowMenuCommand
        {
            get
            {
                if (_showMenuCommand == null)
                {
                    _showMenuCommand = new RelayCommand(p => ShowMenu());
                }
                return _showMenuCommand;
            }
        }

        // Close Menu
        public void CloseMenu()
        {
            IsPanelVisible = false;
        }

        // Close Menu Command
        private ICommand _closeMenuCommand;

        public ICommand CloseMenuCommand
        {
            get
            {
                if (_closeMenuCommand == null)
                {
                    _closeMenuCommand = new RelayCommand(p => CloseMenu());
                }
                return _closeMenuCommand;
            }
        }

        private bool _isPanelVisible;
        public bool IsPanelVisible
        {
            get
            {
                return _isPanelVisible;
            }
            set
            {
                _isPanelVisible = value;
                OnPropertyChanged("IsPanelVisible");
            }
        }

    }
}