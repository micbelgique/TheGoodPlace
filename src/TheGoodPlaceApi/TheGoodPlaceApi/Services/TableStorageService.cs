using Azure.Data.Tables;
using TheGoodPlaceApi.Dto;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using TheGoodPlaceApi.Models;

namespace TheGoodPlaceApi.Services
{
    public class TableStorageService
    {
        private readonly string _connectionString;
        private RoomsDataService _roomsDataService;


        public TableStorageService(IConfiguration configuration, RoomsDataService roomsDataService)
        {
            if (configuration != null)
            {
                _connectionString = configuration.GetConnectionString("DefaultEndpointsProtocol=https;AccountName=saiothubdevices;AccountKey=+OiZhx4mJ7D8RpRf7u/aD8BtHr/nsBMJ2njDmpLXTWyYvzBzygxcAHjZnAubMTFRRGolWbCHdgVE+AStN14Ivg==;EndpointSuffix=core.windows.net");

            }
            else
            {
                _roomsDataService = roomsDataService;
            }
        }


        public List<DevicePayloadRequestDto> GetAllDatas()
        {
           
            TableServiceClient tableServiceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=saiothubdevices;AccountKey=+OiZhx4mJ7D8RpRf7u/aD8BtHr/nsBMJ2njDmpLXTWyYvzBzygxcAHjZnAubMTFRRGolWbCHdgVE+AStN14Ivg==;EndpointSuffix=core.windows.net");
            TableClient tableClient = tableServiceClient.GetTableClient(
                 tableName: "tsIoTdevices"
             );
            var datas = tableClient.Query<DevicePayloadRequestDto>().ToList();
            return datas;
        }

        public List<DevicePayloadRequestDto> GetAllDatasWithHumidity()
        {
            var allData = GetAllDatas(); 

            var latestHumidityDataForEachDevice = allData
                .Where(d => d.value.Contains("humidity"))
                .GroupBy(d => d.deviceName)
                .Select(g => g.OrderByDescending(d => d.eventTime).First())
                .ToList();

            return latestHumidityDataForEachDevice;
        }

        public List<DevicePayloadRequestDto> GetDataForDevice(string deviceName)
        {
            var allData = GetAllDatasWithHumidity();

            var dataForDevice = allData
                .Where(d => d.deviceName == deviceName)
                .ToList();

            return dataForDevice;
        }






    }
}
