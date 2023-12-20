using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response.Remote
{
     public class RemoteType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mainTypeId")]
        public string MainTypeId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }
    }
}
