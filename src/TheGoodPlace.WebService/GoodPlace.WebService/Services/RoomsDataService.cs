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
                Name = "Station",
                PictureUrl = "https://www.mic-belgique.be/rooms/Station",
                DeviceId = "70B3D547501000B5",
                Capacity = 4,

            });

            roomList.Add(new Room
            {
                Name = "Studio",
                PictureUrl = "https://www.mic-belgique.be/rooms/Studio",
                DeviceId = "70B3D54750100259",
                Capacity = 6
            });

            roomList.Add(new Room
            {
                Name = "Cockpit",
                PictureUrl = "https://www.mic-belgique.be/rooms/Cockpit",
                DeviceId = "70B3D54750100291",
                Capacity = 16
            }) ;

            roomList.Add(new Room
            {
                Name = "Loft",
                PictureUrl = "https://www.mic-belgique.be/rooms/Loft",
                DeviceId = "70B3D54750100263",
                Capacity = 8
            });

            roomList.Add(new Room
            {
                Name = "Coffee",
                PictureUrl = "https://www.mic-belgique.be/rooms/Coffee",
                DeviceId = ""
            });

            roomList.Add(new Room
            {
                Name = "Hall",
                PictureUrl = "https://www.mic-belgique.be/rooms/Hall",
                DeviceId = "70B3D54750100252",
                Capacity = 40
            });

            roomList.Add(new Room
            {
                Name = "OpenSpace",
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
