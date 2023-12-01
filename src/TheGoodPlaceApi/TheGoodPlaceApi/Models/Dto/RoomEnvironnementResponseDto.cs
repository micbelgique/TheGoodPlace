using System;
using Azure.Data.Tables;

namespace TheGoodPlaceApi.Models
{
    public class RoomEnvironnementResponseDto
    {
        // Propriétés de Room
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string PictureUrl { get; set; }

        // Caractéristiques de la room
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public int WellnessValue { get; set; }
        public int Justification { get; set; }

    }
}
