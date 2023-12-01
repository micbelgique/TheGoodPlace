using System;
using Azure.Data.Tables;

namespace TheGoodPlaceApi.Models
{
    public class RoomEnvironnementResponseDto
    {
        // Propriétés de Room
        public string Id { get; set; }
        public string Name { get; set; }
        public string Capacity { get; set; }
        public string PictureUrl { get; set; }

        // Caractéristiques de la room
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string WellnessValue { get; set; }
        public string Justification { get; set; }

    }
}
