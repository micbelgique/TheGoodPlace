using Azure.Data.Tables;
using TheGoodPlaceApi.Dto;
using TheGoodPlaceApi.Models;
using TheGoodPlaceApi.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using TheGoodPlaceApi.Models.Dto;

namespace TheGoodPlaceApi.Services
{
    public class RoomsServices
    {
        private RoomsDataService _roomDataService;
        private TableStorageService _dataService;
        private readonly IConfiguration _configuration;

        public RoomsServices(TableStorageService dataService, IConfiguration configuration)
        {
            _roomDataService = new RoomsDataService();
            _dataService = dataService;
            _configuration = configuration;
        }

       public async Task<RoomRankingResponseDto> GetRoomRanking()
        {
            var rooms = _roomDataService.GetRooms().Where(x => !string.IsNullOrEmpty(x.Deviceid)).ToList();
            var datas = _dataService.GetAllDatasWithHumidity();

            string roomDeviceList = "";

            foreach (Room room in rooms)
            {
                var dataForRoom = datas.FirstOrDefault(d => d.deviceName == room.Deviceid);
                if (dataForRoom != null)
                {
                    roomDeviceList += $"- Salle {room.Name} :{dataForRoom.value}, Capacité : {room.Capacity}, PictureUrl: {room.PictureUrl}";
                }
                Console.WriteLine($"RoomDeviceList: {roomDeviceList}");
            }

            string systemPrompt = "Calcule la wellnesvalue c'est un nombre de 1 à 100 du bien etre, plus les temperature, l'humidité et la pression sont propice à travailler plus sa wellness value sera grande";

            string userPrompt = $"Voici mes {rooms.Count} salles :[ {roomDeviceList}].renvoit le tout dans un json format ou wellnessvalue et justification sont toujours remplis";
                

            string apiKey = _configuration.GetValue<string>("ConnectionStrings:OpenAiApiKey");
            string apiUrl = _configuration.GetValue<string>("ConnectionStrings:OpenAiEndpoint");

            IOpenAiService openAiService = new OpenAiService(apiKey, apiUrl);
            var ranking = await openAiService.GetRoomRanking(systemPrompt, userPrompt);

            // AJouter un traitement de tri + (quelques verifications)

            return ranking;
        }

    }
}