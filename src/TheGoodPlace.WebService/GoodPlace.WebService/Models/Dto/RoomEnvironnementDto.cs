﻿using System;
using System.Collections.Generic;

namespace GoodPlace.WebService.Dto
{
    public class RoomRankingDto
    {
        public RoomEnvironnementDto TheGoodPlace { get; set; }
        public List<RoomEnvironnementDto> Rooms { get; set; }
    }

    public class RoomEnvironnementDto
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string DeviceId { get; set; }
        public float WellnessValue { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public int Luminosity { get; set; }
        public DateTime LastSync { get; set; }
    }
}
