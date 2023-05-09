using System;
using Azure;
using Azure.Data.Tables;

namespace GoodPlace.WebService.Dto
{
    public class Payload: ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string PayloadId { get; set; }
        public string deviceName { get; set; }
        public string devEUI { get; set; }
        public string devType { get; set; }
        public string container { get; set; }
        public string value { get; set; }
        public DateTime date { get; set; }
        public ETag ETag { get; set; }
        DateTimeOffset? ITableEntity.Timestamp { get; set; }
    }
}
