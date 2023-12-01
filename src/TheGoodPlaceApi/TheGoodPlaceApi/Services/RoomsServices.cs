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

            string roomList = "";
            foreach (Room room in rooms)
            {
                // Faire le matching room -> DeviceID
                // Ancien code : //var devicePayload = this.getLastrecordsFromSpecificDevice(datas, room.Deviceid);
        //        roomList += $"La salle {room.Name} :DeviceId {room.Deviceid} : température {devicePayload.Temperature} degrés, luminosité {devicePayload.Luminosity} lumens, humidité {devicePayloads.Humidity}%, lastSync {devicePayloads.LastSync}";
            }

            string prompt = $"Je souhaite réserver une salle pour une réunion. J'ai le choix entre {rooms.Count} salles : {roomList}. " +
                            $"Quelle salle me conseilles-tu et pourquoi ? Renvoie-moi le résultat sous forme d'une liste JSON respectant ce format : \"Name\" (le nom de la salle sans le mot \"salle\"),\"DeviceId\" (l'id de la salle), \"WellnessValue\" (en pourcentage), \"Temperature\", \"Humidity\", \"Luminosity\", \"Justification\", \"lastSync\" (en une ligne). Réponds uniquement dans le format JSON. Toutes les salles doivent être présentes dans la liste et seront triées par ordre décroissant de wellnessValue.";

            string apiKey = _configuration.GetValue<string>("ConnectionStrings:OpenAiApiKey");
            string apiUrl = "https://api.openai.com/v1/completions";

            IOpenAiService openAiService = new OpenAiService(apiKey, apiUrl);
            var ranking = await openAiService.GetRoomRanking(prompt);

            // AJouter un traitement de tri + (quelques verifications)

            return ranking;
        }

    }
}