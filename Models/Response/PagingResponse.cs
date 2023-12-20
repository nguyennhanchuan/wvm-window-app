using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response
{
    public class PagingResponse<T>
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }

    public class DataResponse<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
}
