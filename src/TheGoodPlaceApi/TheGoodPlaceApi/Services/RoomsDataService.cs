using TheGoodPlaceApi.Models;
using System;
using System.Collections.Generic;

namespace TheGoodPlaceApi.Services
{
    public class RoomsDataService
    {
        public List<Room> GetRooms()
        {
            List<Room> roomList = new List<Room>();

            //roomList.Add(new Room
            //{
            //    Name = "Station",
            //    PictureUrl = "https://www.mic-belgique.be/rooms/Station",
            //    DeviceName = "70B3D547501000B5",
            //    Capacity = 4,

            //});
       
            roomList.Add(new Room
            {
                Name = "Studio",
                PictureUrl = "https://www.mic-belgique.be/rooms/Studio",
                Deviceid = "MXChip-002",
                Capacity = 6
            });

         
            roomList.Add(new Room
            {
                Name = "Cockpit",
                PictureUrl = "https://www.mic-belgique.be/rooms/Cockpit",
                Deviceid = "MXChip-001",
                Capacity = 16
            }) ;

         
            roomList.Add(new Room
            {
                Name = "Loft",
                PictureUrl = "https://www.mic-belgique.be/rooms/Loft",
                Deviceid = "MXChip-003",
                Capacity = 8
            });

            //roomList.Add(new Room
            //{
            //    Name = "Coffee",
            //    PictureUrl = "https://www.mic-belgique.be/rooms/Coffee",
            //    DeviceName = ""
            //});
            //here
            roomList.Add(new Room
            {
                Name = "Hall",
                PictureUrl = "https://www.mic-belgique.be/rooms/Hall",
                Deviceid= "MXChip-004",
                Capacity = 40
            });

            //roomList.Add(new Room
            //{
            //    Name = "OpenSpace",
            //    PictureUrl = "https://www.mic-belgique.be/rooms/OpenSpace",
            //    DeviceName = ""
            //});

            //roomList.Add(new Room
            //{
            //    Name = "L'espace Softlab",
            //    PictureUrl = "",
            //    DeviceName = ""
            //});


            return roomList;
        }
    }
}
