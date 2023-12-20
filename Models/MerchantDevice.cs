using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class MerchantDevice
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string MerchantStoreId { get; set; }

        public string MerchantId { get; set; }
    }
}
