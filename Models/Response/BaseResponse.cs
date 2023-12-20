using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response
{
    public class BaseResponse<T>
    {
        [JsonPropertyName("result")]
        public Result<T>? Result { get; set; }

        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("massage")]
        public string Message { get; set; }
    }
    public class Result<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
