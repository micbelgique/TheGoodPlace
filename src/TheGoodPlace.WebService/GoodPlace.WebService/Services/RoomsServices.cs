using Azure.Data.Tables;
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

        public RoomRankingDto GetRoomRanking()
        {
            var environnements = this.CreateRankingFromPayloads()
                                     .OrderBy(x => x.WellnessValue)
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
            string endpoint = "";
            string key = "";

            string engine = "gpt4";

            OpenAIClient client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

            string prompt = "Sur base du modèle json  peux-tu me choisir la meilleur pièce à vivre et justifie moi en 2 lignes de manière  scientifique.json : " + rankingRoom;
            Console.Write($"Input: {prompt}\n");

            Response<Completions> completionsResponse =
                client.GetCompletions(engine, prompt);
            string completion = completionsResponse.Value.Choices[0].Text;

            Console.WriteLine($"Chatbot: {completion}");
   
            return completion;
        }

        public RoomRankingWithJustificationDto GetRoomRankingWithJustification()
        {
            var environnements = this.CreateRankingFromPayloads()
                                     .OrderBy(x => x.WellnessValue)
                                     .ToList();

            var goodPlace = environnements.First();
            environnements.Remove(goodPlace);

            RoomRankingWithJustificationDto rankingWithJustification = new RoomRankingWithJustificationDto
            {
                Rooms = environnements,
                TheGoodPlace = goodPlace,
                Justification = GetJustification()
            };

            return rankingWithJustification;
        }

        public List<RoomEnvironnementDto> CreateRankingFromPayloads() 
        {
            // We get the rooms from mock service
            var rooms = _roomDataService.GetRooms();

            // We get a list of all room in rankedRoom format
            var environnements = this.MapRoomsIntoEnvironnements(rooms);

            var datas = _dataService.GetRecentRecords(DateTime.Now.AddDays(-400));

            // We associate the values of each devices in the right room
            foreach(RoomEnvironnementDto environnement in environnements)
            {
                var devicePayloads = this.getLastrecordsFromSpecificDevice(datas, environnement.DeviceId);
                environnement.Temperature = devicePayloads.Temperature;
                environnement.Humidity = devicePayloads.Humidity;
                environnement.Luminosity = devicePayloads.Luminosity;
                environnement.LastSync = devicePayloads.LastSync;

                // Crazy space futuristic environnement formula
                environnement.WellnessValue = (Math.Abs(environnement.Temperature - 21) * 3) + Math.Abs(environnement.Humidity - 50);
            }

            return environnements;
        }

        private List<RoomEnvironnementDto> MapRoomsIntoEnvironnements(List<Room> rooms)
        {
            List<RoomEnvironnementDto> rankedRooms = new List<RoomEnvironnementDto>();
            foreach (Room room in rooms)
            {
                if (room.DeviceId != "")
                {
                    rankedRooms.Add(
                    new RoomEnvironnementDto
                    {
                        Name = room.Name,
                        PictureUrl = room.PictureUrl,
                        DeviceId = room.DeviceId
                    }
                    );
                }
            }

            return rankedRooms;
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
