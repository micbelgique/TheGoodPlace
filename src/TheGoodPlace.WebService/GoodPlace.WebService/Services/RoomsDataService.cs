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
                PictureUrl = "",
                DeviceId = "70B3D547501000B5"
            });

            roomList.Add(new Room
            {
                Name = "Le Studio",
                PictureUrl = "",
                DeviceId = "70B3D54750100259"
            });

            roomList.Add(new Room
            {
                Name = "Le Cockpit",
                PictureUrl = "",
                DeviceId = "70B3D54750100291"
            });

            roomList.Add(new Room
            {
                Name = "Le Loft",
                PictureUrl = "",
                DeviceId = "70B3D54750100263"
            });

            roomList.Add(new Room
            {
                Name = "L'espace Network",
                PictureUrl = "",
                DeviceId = "70B3D54750100252"
            });

            roomList.Add(new Room
            {
                Name = "Le Hall",
                PictureUrl = "",
                DeviceId = ""
            });

            roomList.Add(new Room
            {
                Name = "L'espace Workplace",
                PictureUrl = "",
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
