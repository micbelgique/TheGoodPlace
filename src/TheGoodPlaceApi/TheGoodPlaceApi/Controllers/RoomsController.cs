using TheGoodPlaceApi.Dto;
using TheGoodPlaceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TheGoodPlaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomsServices _roomsServices;
        public RoomsController(RoomsServices roomsServices)
        {
            this._roomsServices = roomsServices;
        }
        [HttpGet("ranking")]
        [Produces("application/json", Type = null)]
        public async Task<IActionResult> GetRoomRanking()
        {
            var ranking = await _roomsServices.GetRoomRanking();
            ranking.salles = ranking.salles.OrderByDescending(s => int.Parse(s.WellnessValue)).ToList();
            return Ok(ranking);
        }


    }
}
