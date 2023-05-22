using GoodPlace.WebService.Dto;
using GoodPlace.WebService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodPlace.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly RoomsServices _roomsServices;
        public RoomsController(RoomsServices roomsServices)
        {
            this._roomsServices = roomsServices;
        }
        [HttpGet("ranking")]
        [Produces("application/json", Type = null)]
        public IActionResult GetRoomRanking()
        {
            var ranking = _roomsServices.GetRoomRanking();
            return Ok(ranking);
        }
    }
}
