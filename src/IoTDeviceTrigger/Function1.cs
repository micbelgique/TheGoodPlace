using Azure.Messaging.EventHubs;
using IoTDeviceTrigger.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceTrigger
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run([EventHubTrigger("samples-workitems", Connection = "ConnectionString")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            string storageConnectionString = Environment.GetEnvironmentVariable("ConnectionStringStorageaccount");
            string tableName = "tsIoTdevices";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            foreach (EventData eventData in events)
            {
                try
                {
                    string payload = Encoding.UTF8.GetString(eventData.Body.ToArray());
                    var deviceId = GetDeviceId(eventData);
                    log.LogInformation($"Received message: {payload}");

                   
                    MyTableEntity entity = new MyTableEntity
                    {
                        PartitionKey = Guid.NewGuid().ToString(),
                        RowKey = Guid.NewGuid().ToString(),
                        PayloadId = Guid.NewGuid().ToString(),
                        eventTime = eventData.EnqueuedTime.ToString(),
                        value = payload,
                        deviceName = deviceId,
                        
 
                    };

                    TableOperation insertOperation = TableOperation.Insert(entity);
                    await table.ExecuteAsync(insertOperation);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }

        public static string GetDeviceId(EventData message)
        {
            return message.SystemProperties["iothub-connection-device-id"].ToString();
        }
    }


   
}
