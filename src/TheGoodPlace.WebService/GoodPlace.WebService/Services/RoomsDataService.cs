using GoodPlace.WebService.Models;
using System;
using System.Collections.Generic;

namespace GoodPlace.WebService.Services
{
    public class RoomsDataService
    {
        public List<Room> GetRooms()
        {
            List<Room> roomList = new List<Room>();

            roomList.Add(new Room
            {
                Name = "La Station",
                PictureUrl = "https://www.mic-belgique.be/rooms/Station",
                DeviceId = "70B3D547501000B5"
            });

            roomList.Add(new Room
            {
                Name = "Le Studio",
                PictureUrl = "https://www.mic-belgique.be/rooms/Studio",
                DeviceId = "70B3D54750100259"
            });

            roomList.Add(new Room
            {
                Name = "Le Cockpit",
                PictureUrl = "https://www.mic-belgique.be/rooms/Cockpit",
                DeviceId = "70B3D54750100291"
            });

            roomList.Add(new Room
            {
                Name = "Le Loft",
                PictureUrl = "https://www.mic-belgique.be/rooms/Loft",
                DeviceId = "70B3D54750100263"
            });

            roomList.Add(new Room
            {
                Name = "L'espace Network",
                PictureUrl = "https://www.mic-belgique.be/rooms/Coffee",
                DeviceId = ""
            });

            roomList.Add(new Room
            {
                Name = "Le Hall",
                PictureUrl = "https://www.mic-belgique.be/rooms/Hall",
                DeviceId = "70B3D54750100252"
            });

            roomList.Add(new Room
            {
                Name = "L'espace Workplace",
                PictureUrl = "https://www.mic-belgique.be/rooms/OpenSpace",
                DeviceId = ""
            });

            roomList.Add(new Room
            {
                Name = "L'espace Softlab",
                PictureUrl = "",
                DeviceId = ""
            });


            return roomList;
        }
    }
}
