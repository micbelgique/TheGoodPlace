using System;
using Azure;
using Azure.Data.Tables;

namespace TheGoodPlaceApi.Dto
{
    public class DevicePayloadRequestDto : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Timestamp { get; set; }
        public string PayloadId { get; set; }
        public string deviceName { get; set; }
        public string eventTime { get; set; }
        public string value { get; set; }
        public ETag ETag { get; set; }
        DateTimeOffset? ITableEntity.Timestamp { get; set; }
    }
}
