using System;
using System.Collections.Generic;

namespace GoodPlace.WebService.Dto
{
    public class RoomRankingWithJustificationDto
    {
        public RoomEnvironnementDto TheGoodPlace { get; set; }
        public List<RoomEnvironnementDto> Rooms { get; set; }
        public string Justification { get; set; }
    }
}
