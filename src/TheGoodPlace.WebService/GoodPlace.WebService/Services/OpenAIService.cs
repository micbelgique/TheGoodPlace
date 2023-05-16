using Microsoft.Extensions.Configuration;
using System.Threading;

namespace GoodPlace.WebService.Services
{
    public class OpenAIService
    {
        //private RoomsServices _roomsService;

        //public OpenAIService(RoomsServices roomsServices)
        //{
        //    _roomsService = roomsServices;
        //}

        public string GetJustification()
        {
            string justification = "Je suis la justification";
            return justification;
        }

    }
}
