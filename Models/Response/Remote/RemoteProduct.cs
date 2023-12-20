using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response.Remote
{
    public class RemoteProduct
    {
        [JsonProperty("isAllowUpdate")]
        public bool IsAllowUpdate { get; set; }

        [JsonProperty("isAllowDelete")]
        public bool IsAllowDelete { get; set; }

        [JsonProperty("isAllowChangeActive")]
        public bool IsAllowChangeActive { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("merchantProductId")]
        public string MerchantProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("productTypes")]
        public List<RemoteType> ProductTypes { get; set; }

        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedBy")]
        public int ModifiedBy { get; set; }

        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}
