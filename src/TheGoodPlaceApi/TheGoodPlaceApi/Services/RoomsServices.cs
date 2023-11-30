using Azure.Data.Tables;
using TheGoodPlaceApi.Dto;
using TheGoodPlaceApi.Models;
using TheGoodPlaceApi.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheGoodPlaceApi.Services
{
    public class RoomsServices
    {
        private RoomsDataService _roomDataService;
        private DataService _dataService;
        private readonly IConfiguration _configuration;

        public RoomsServices(DataService dataService, IConfiguration configuration)
        {
            _roomDataService = new RoomsDataService();
            _dataService = dataService;
            _configuration = configuration;
        }

        public async Task<RoomRankingDto> GetRoomRanking()
        {
            List<RoomEnvironnementDto> environnements = await this.CreateRankingFromPayloads();

            //environnements.OrderBy(x => x.WellnessValue)
            //               .ToList();

            var goodPlace = environnements.First();
            environnements.Remove(goodPlace);

            RoomRankingDto ranking = new RoomRankingDto
            {
                Rooms = environnements,
                TheGoodPlace = goodPlace,
            };

            return ranking;
        }

       public async Task<List<RoomEnvironnementDto>> CreateRankingFromPayloads()
        {
            var rooms = _roomDataService.GetRooms().Where(x => !string.IsNullOrEmpty(x.DeviceId)).ToList();
            var datas = _dataService.GetRecentRecords(DateTime.Now.AddDays(-10));

            string roomList = "";
            foreach (Room room in rooms)
            {
                var devicePayloads = this.getLastrecordsFromSpecificDevice(datas, room.DeviceId);
                roomList += $"La salle {room.Name} :DeviceId {room.DeviceId} : température {devicePayloads.Temperature} degrés, luminosité {devicePayloads.Luminosity} lumens, humidité {devicePayloads.Humidity}%, lastSync {devicePayloads.LastSync}";
            }

            string prompt = $"Je souhaite réserver une salle pour une réunion. J'ai le choix entre {rooms.Count} salles : {roomList}. " +
                            $"Quelle salle me conseilles-tu et pourquoi ? Renvoie-moi le résultat sous forme d'une liste JSON respectant ce format : \"Name\" (le nom de la salle sans le mot \"salle\"),\"DeviceId\" (l'id de la salle), \"WellnessValue\" (en pourcentage), \"Temperature\", \"Humidity\", \"Luminosity\", \"Justification\", \"lastSync\" (en une ligne). Réponds uniquement dans le format JSON. Toutes les salles doivent être présentes dans la liste et seront triées par ordre décroissant de wellnessValue.";

            string apiKey = _configuration.GetValue<string>("ConnectionStrings:OpenAiApiKey");
            string apiUrl = "https://api.openai.com/v1/completions";

            IOpenAiService openAiService = new OpenAiService(apiKey, apiUrl);
            string result = await openAiService.GenerateTextAsync(prompt);

            List<RoomEnvironnementDto> recommendationList = null;

            string jsonToDeserialize = result;
            recommendationList = JsonSerializer.Deserialize<List<RoomEnvironnementDto>>(jsonToDeserialize);

            return recommendationList;
        }





        private List<RoomEnvironnementDto> MapRoomsIntoEnvironnements(List<Room> rooms)
        {
            List<RoomEnvironnementDto> environnement = new List<RoomEnvironnementDto>();
            foreach (Room room in rooms)
            {
                if (room.DeviceId != "")
                {
                    environnement.Add(
                    new RoomEnvironnementDto
                    {
                        Name = room.Name,
                        PictureUrl = room.PictureUrl,
                        DeviceId = room.DeviceId,
                        Capacity = room.Capacity
                    }
                    );
                }
            }

            return environnement;
        }

        public RoomEnvironnementDto getLastrecordsFromSpecificDevice(List<Payload> data, string deviceId)
        
        {
            // Query  by deviceId

            var lastRecordByDeviceTemp = data
                 .Where(x
                     => x.devEUI == deviceId
                     && x.container == "temperature"
                     )
                 .OrderByDescending(x => x.date)
                 .FirstOrDefault();

            var lastRecordByDeviceHum = data
                 .Where(x
                     => x.devEUI == deviceId
                     && x.container == "humidity"
                     )
                 .OrderByDescending(x => x.date)
                 .FirstOrDefault();

            var lastRecordByDeviceLum = data
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
                Luminosity = int.Parse(lastRecordByDeviceLum.value, CultureInfo.InvariantCulture.NumberFormat),
                LastSync = lastRecordByDeviceTemp.date
            };

            return roomEnvironnement;
        }
    }
}