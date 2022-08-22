using Azure.Data.Tables;
using GoodPlace.WebService.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GoodPlace.WebService.Services
{
    public class DataService
    {
        private IConfiguration _configuration;
        private RoomsDataService _roomDataService;

        public DataService(IConfiguration configuration, RoomsDataService roomDataService)
        {
            _configuration = configuration;
            _roomDataService = roomDataService;
        }

        public List<Payload> GetAllDatas()
        {
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(_configuration.GetConnectionString("MyStorageConnection"));

            // New instance of TableClient class referencing the server-side table
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "DevicesDataTable"
            ) ;

            var datas = tableClient.Query<Payload>().ToList();
            return datas;
        }

        public List<Payload> GetRecentRecords(DateTime minDate)
        {
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(_configuration.GetConnectionString("MyStorageConnection"));

            // New instance of TableClient class referencing the server-side table
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "DevicesDataTable"
            );

            var datas = tableClient.Query<Payload>(x => x.date > minDate).ToList();
            return datas;
        }

        public IEnumerable<Payload> GetAllDatasByDevice(string deviceId)
        {
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(_configuration.GetConnectionString("MyStorageConnection"));

            // New instance of TableClient class referencing the server-side table
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "DevicesDataTable"
            );

            var datasByDevices = tableClient.Query<Payload>((x => x.devEUI == deviceId));
            return datasByDevices;
        }

        public IEnumerable<Payload> GetAllDatasByDeviceAndContainer(string deviceId, string deviceContainer)
        {
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(_configuration.GetConnectionString("MyStorageConnection"));

            // New instance of TableClient class referencing the server-side table
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "DevicesDataTable"
            );

            var datasByDevicesAndContainer = tableClient.Query<Payload>((x => x.devEUI == deviceId && x.container == deviceContainer));
            return datasByDevicesAndContainer;
        }

        public IEnumerable<Payload> Lastrecord(string deviceId, string deviceContainer)
        {
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(_configuration.GetConnectionString("MyStorageConnection"));

            // New instance of TableClient class referencing the server-side table
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "DevicesDataTable"
            );

            var lastRecordByDeviceAndContainer = tableClient.Query<Payload>(x => x.devEUI == deviceId && x.container == deviceContainer,maxPerPage: 1).FirstOrDefault();
            yield return lastRecordByDeviceAndContainer;
        }

        public RoomEnvironnementDto LastrecordsFromSpecificDevice(string deviceId)
        { 
           // deviceId = "70B3D54750100259";

            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=sathegoodplace0722;AccountKey=4jL1lVaThaxHFsx9rvUVQ6K2xJhV+FUe/r3GfgHZQDnI6qNVOzyqrkrn4sTKGzEEoxJAuib2nFzw+AStZhl+YA==;EndpointSuffix=core.windows.net");

            // New instance of TableClient class referencing the server-side table
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "DevicesDataTable"
            );

            // Query  by deviceId

            var lastRecordByDeviceTemp = tableClient.Query<Payload>()
                 .Where(x
                     => x.devEUI == deviceId
                     && x.container == "temperature"
                     )
                 .OrderByDescending(x => x.date)
                 .FirstOrDefault();

            var lastRecordByDeviceHum = tableClient.Query<Payload>()
                 .Where(x
                     => x.devEUI == deviceId
                     && x.container == "humidity"
                     )
                 .OrderByDescending(x => x.date)
                 .FirstOrDefault();

            //var lastRecordByDevicePres = tableClient.Query<Payload>()
            //     .Where(x
            //         => x.devEUI == deviceId
            //         && x.container == "presence"
            //         )
            //     .OrderByDescending(x => x.date)
            //     .FirstOrDefault();

            var lastRecordByDeviceLum = tableClient.Query<Payload>()
                 .Where(x
                     => x.devEUI == deviceId
                     && x.container == "luminosity"
                     )
                 .OrderByDescending(x => x.date)
                 .FirstOrDefault();

            RoomEnvironnementDto roomEnvironnement = new RoomEnvironnementDto
            {
                DeviceId = deviceId,
                Temperature = float.Parse(lastRecordByDeviceTemp.value, CultureInfo.InvariantCulture.NumberFormat),
                Humidity = float.Parse(lastRecordByDeviceHum.value, CultureInfo.InvariantCulture.NumberFormat),
                Luminosity = int.Parse(lastRecordByDeviceLum.value, CultureInfo.InvariantCulture.NumberFormat)
            };

        

            return roomEnvironnement;
        }
    }
}
