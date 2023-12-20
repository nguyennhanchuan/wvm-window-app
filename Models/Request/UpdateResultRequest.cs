using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models.Request
{
    public class ProductResultPosition
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

    public class ProductResult
    {
        [JsonProperty("merchantProductId")]
        public string MerchantProductId { get; set; }

        [JsonProperty("positions")]
        public List<ProductResultPosition> Positions { get; set; }

        public ProductResult() {
            Positions = new List<ProductResultPosition>();
        }
    }

    public class UpdateResultRequest
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }

        [JsonProperty("products")]
        public List<ProductResult> Products { get; set; }
    }
}
