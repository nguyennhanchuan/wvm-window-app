using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response.Remote
{
    public class OrderDetail
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("oldPrice")]
        public double OldPrice { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("positions")]
        public List<Position> Positions { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class Position
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("row")]
        public int Row { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("totalPayment")]
        public decimal TotalPayment { get; set; }

        [JsonProperty("totalDiscount")]
        public decimal TotalDiscount { get; set; }

        [JsonProperty("totalService")]
        public decimal TotalService { get; set; }

        [JsonProperty("totalShipping")]
        public decimal TotalShipping { get; set; }

        [JsonProperty("totalVat")]
        public decimal TotalVat { get; set; }

        [JsonProperty("vatnumber")]
        public string Vatnumber { get; set; }

        [JsonProperty("refundAmount")]
        public double RefundAmount { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("cardTypeId")]
        public string CardTypeId { get; set; }

        [JsonProperty("cardTypeName")]
        public string CardTypeName { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("storeName")]
        public string StoreName { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        [JsonProperty("shippingAddressId")]
        public string ShippingAddressId { get; set; }

        [JsonProperty("pickupAddressId")]
        public string PickupAddressId { get; set; }

        [JsonProperty("paymentTypeId")]
        public string PaymentTypeId { get; set; }

        [JsonProperty("paymentTypeName")]
        public string PaymentTypeName { get; set; }

        [JsonProperty("shippingTypeId")]
        public string ShippingTypeId { get; set; }

        [JsonProperty("shippingTypeName")]
        public string ShippingTypeName { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("statusId")]
        public string StatusId { get; set; }
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("statusName")]
        public string StatusName { get; set; }

        [JsonProperty("paymentStatusId")]
        public string PaymentStatusId { get; set; }

        [JsonProperty("paymentStatusName")]
        public string PaymentStatusName { get; set; }


        [JsonProperty("paymentStatusCode")]
        public string PaymentStatusCode { get; set; }

        [JsonProperty("shippingStatusId")]
        public string ShippingStatusId { get; set; }

        [JsonProperty("shippingStatusName")]
        public string ShippingStatusName { get; set; }

        [JsonProperty("canceledDate")]
        public DateTime? CanceledDate { get; set; }

        [JsonProperty("canceledReason")]
        public string CanceledReason { get; set; }

        [JsonProperty("orderDetails")]
        public List<OrderDetail> OrderDetails { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }

        [JsonProperty("qrCode")]
        public string QrCode { get; set; }


        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public static class OmsOrderStatus
    {
        public const string New = "NEW";
        public const string Pending = "PEN";
        public const string WaitingForPayment = "WAI";
        public const string Paymented = "PAY";
        public const string Cancelled = "CAN";
        public const string Refunded = "REF";
    }

    public static class PaymentStatus
    {
        public const string No = "NO";
        public const string Yes = "YES";
    }
}
