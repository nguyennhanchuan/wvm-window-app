using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VendingMachine.Models.Request
{
    internal class GetTokenRequest : BaseRequest
    {

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("passWord")]
        public string PassWord { get; set; }

        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("deviceCode")]
        public string DeviceCode { get; set; }

    }
}
