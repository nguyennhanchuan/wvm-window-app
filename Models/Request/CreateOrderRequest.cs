using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models.Request
{
    public class ProductDetail
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class CreateOrderRequest

    {
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("vatnumber")]
        public string Vatnumber { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("cardTypeId")]
        public string CardTypeId { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("shippingAddressId")]
        public string ShippingAddressId { get; set; }

        [JsonProperty("pickupAddressId")]
        public string PickupAddressId { get; set; }

        [JsonProperty("paymentTypeId")]
        public string PaymentTypeId { get; set; }

        [JsonProperty("shippingTypeId")]
        public string ShippingTypeId { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("orderDetails")]
        public List<ProductDetail> OrderDetails { get; set; }
    }
}
