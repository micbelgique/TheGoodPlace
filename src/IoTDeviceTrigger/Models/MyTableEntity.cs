using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceTrigger.Models
{
    internal class MyTableEntity : TableEntity
    {
        public string PayloadId { get; set; }
        public string deviceName { get; set; }
        public string devEUI { get; set; }
        public string eventTime { get; set; }
        public string value { get; set; }
    }
}

