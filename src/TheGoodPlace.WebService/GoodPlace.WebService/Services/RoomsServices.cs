﻿using Azure.Data.Tables;
using GoodPlace.WebService.Dto;
using GoodPlace.WebService.Models;
using GoodPlace.WebService.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

//OpenAI Using

using Azure;
using Azure.AI.OpenAI;
using static System.Environment;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Collections;

namespace GoodPlace.WebService.Services
{
    public class RoomsServices
    {
        private RoomsDataService _roomDataService;
        private DataService _dataService;

        public RoomsServices(DataService dataService, IConfiguration configuration)
        {
            _roomDataService = new RoomsDataService();
            _dataService = dataService;
        }

        public async Task<RoomRankingDto> GetRoomRanking()
        {
            List<RoomEnvironnementDto> environnements = await this.CreateRankingFromPayloads();

            environnements.OrderBy(x => x.WellnessValue)
                           .ToList();

            var goodPlace = environnements.First();
            environnements.Remove(goodPlace);

            RoomRankingDto ranking = new RoomRankingDto
            {
                Rooms = environnements,
                TheGoodPlace = goodPlace,
            };

            return ranking;
        }

        public string GetJustification()
        {
            string rankingRoom = JsonConvert.SerializeObject(this.GetRoomRanking());
            string endpoint = "https://goodplaceopenai.openai.azure.com/";
            string key = "182533ff6c92423985d5476424f2fe8d";

            string engine = "text";

            OpenAIClient client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

            string prompt = "En moins de 50 mots justifie moi de manière simple et scientifique pourquoi la Studio est la meilleur piece pour travailler tu peux te baser sur ce json pour y repondre sans mentionner le format json" ;
            Console.Write($"Input: {prompt}\n");


            Response<Completions> completionsResponse =
                client.GetCompletions(engine , prompt);
            string completion = completionsResponse.Value.Choices[0].Text;

            Console.WriteLine($"Chatbot: {completion}");
   
            return completion;
        }

        //public RoomRankingWithJustificationDto GetRoomRankingWithJustification()
        //{
        //    var environnements = this.CreateRankingFromPayloads()
        //                             .OrderBy(x => x.WellnessValue)
        //                             .ToList();

        //    var goodPlace = environnements.First();
        //    environnements.Remove(goodPlace);

        //    RoomRankingWithJustificationDto rankingWithJustification = new RoomRankingWithJustificationDto
        //    {
        //        Rooms = environnements,
        //        TheGoodPlace = goodPlace,
        //        Justification = GetJustification()
        //    };

        //    return rankingWithJustification;
        //}

        public async Task<List<RoomEnvironnementDto>> CreateRankingFromPayloads()
        {
            // Récupérer les salles depuis le service mock
            var rooms = _roomDataService.GetRooms().Where(x => !string.IsNullOrEmpty(x.DeviceId)).ToList();
            var datas = _dataService.GetRecentRecords(DateTime.Now.AddDays(-400));

            // Associer les valeurs de chaque appareil à la salle correspondante
            string roomList = "";
            foreach (Room room in rooms)
            {
                var devicePayloads = this.getLastrecordsFromSpecificDevice(datas, room.DeviceId);
                roomList += $"La salle {room.Name} : température {devicePayloads.Temperature} degrés, luminosité {devicePayloads.Luminosity} lumens, humidité {devicePayloads.Humidity}%";
            }

            // Ajouter le préprompt
            string prompt = $"Je souhaite réserver une salle pour une réunion. J'ai le choix entre {rooms.Count} salles : {roomList}. " +
                            $"Quelle salle me conseilles-tu et pourquoi ? Renvoie-moi le résultat sous forme d'une liste JSON respectant ce format : \"Name\" (le nom de la salle sans le mot \"salle\"), \"WellnessValue\" (en pourcentage), \"Temperature\", \"Humidity\", \"Luminosity\", \"Justification\" (en une ligne). Réponds uniquement dans le format JSON. Toutes les salles doivent être présentes dans la liste et seront triées par ordre décroissant de wellnessValue.";

            // OpenAI
            string apiKey = "";
            string apiUrl = "";
            string result = "";

            dynamic requestBody = new
            {
                prompt = prompt,
                max_tokens = 800 // Le nombre maximum de tokens à générer
            };
            string requestBodyJson = JsonConvert.SerializeObject(requestBody);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);
              
                string responseJson = await response.Content.ReadAsStringAsync();

                JObject responseData = JObject.Parse(responseJson);
                JToken choicesToken = responseData["choices"];
                if (choicesToken != null && choicesToken.HasValues)
                {
                    result = choicesToken[0]["text"].Value<string>().Trim('\n', '"');
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("Une erreur s'est produite lors de la génération du texte.");
                }

                List<RoomEnvironnementDto> recommendationList = null;

                try
                {
                    string jsonToDeserialize = result.Trim('.', ' ', '\n');
                    recommendationList = JsonConvert.DeserializeObject<List<RoomEnvironnementDto>>(jsonToDeserialize);
                    Console.WriteLine("hello" + recommendationList);
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Une erreur s'est produite lors de la désérialisation de la réponse JSON : {jsonEx.Message}");
                }

                return recommendationList;
            }
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
