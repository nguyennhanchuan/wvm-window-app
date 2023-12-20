using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models.Request
{
    public class CheckPaymentRequest
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }
    }
}
