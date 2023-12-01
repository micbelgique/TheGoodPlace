using System.Collections.Generic;

namespace TheGoodPlaceApi.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string PictureUrl { get; set; }
        public string Deviceid { get; set; }
    }
}
