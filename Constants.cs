using System;

namespace VendingMachine
{
    public static class Constants
    {
        public static class UI
        {
            public const string ExceptionTitle = "whoops - something's gone wrong!";

            public static class Diagnostics
            {
                public const string DefaultCpuString = "CPU: 00 %";
                public const string DefaultManagedMemoryString = "Managed Memory: 00 Mb";
                public const string DefaultTotalMemoryString = "Total Memory: 00 Mb";
                public static readonly TimeSpan Heartbeat = TimeSpan.FromSeconds(5);
                public static readonly TimeSpan UiFreeze = TimeSpan.FromMilliseconds(500);
                public static readonly TimeSpan UiFreezeTimer = TimeSpan.FromMilliseconds(333);

                public static readonly TimeSpan DiagnosticsLogInterval = TimeSpan.FromSeconds(1);
                public static readonly TimeSpan DiagnosticsIdleBuffer = TimeSpan.FromMilliseconds(666);
                public static readonly TimeSpan DiagnosticsCpuBuffer = TimeSpan.FromMilliseconds(666);
                public static readonly TimeSpan DiagnosticsSubscriptionDelay = TimeSpan.FromMilliseconds(1000);
            }
        }

        public static class Server
        {
            public static readonly string BaseUri = $"https://localhost:5294/";
            public static readonly string DeviceUri = $"http://wvm-apimobile.microteam.asia/";


            public static readonly Uri Login = new Uri(DeviceUri + "api/SysUser/login");
            public static readonly Uri RefreshToken = new Uri(BaseUri + "api/SysUser/refresh-token");
            public static readonly Uri MainType = new Uri(DeviceUri + "api/MainType/product-type");
            public static readonly Uri MainProduct = new Uri(DeviceUri + "api/MainMerchantProductDevice/all");
            public static readonly Uri CreateOrder = new Uri(DeviceUri + "api/OmsOrder");
            public static readonly Uri UpdateOrderResult = new Uri(DeviceUri + "api/OmsOrder/update-result");
            public static readonly Uri CheckPayment = new Uri(DeviceUri + "api/OmsOrder/check-payment");
            public static readonly Uri GetPaymentMethods = new Uri(DeviceUri + "api/MainType/payment-methods");
        }
    }
}