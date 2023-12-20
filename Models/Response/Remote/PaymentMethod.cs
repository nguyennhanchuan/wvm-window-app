using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response.Remote
{
    public class PaymentMethod
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        public bool IsSelected { get; set; }
    
    }
}
