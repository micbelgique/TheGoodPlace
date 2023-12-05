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
        [HttpGet("highest-room")]
        [Produces("application/json", Type = null)]
        public async Task<IActionResult> GetHighestRankingRoom()
        {
            var ranking = await _roomsServices.GetRoomRanking();
            var highestRankingRoom = ranking.salles.OrderByDescending(s => int.Parse(s.WellnessValue)).FirstOrDefault();

            return Ok(highestRankingRoom);
        }
        [HttpGet("other-rooms")]
        [Produces("application/json", Type = null)]
        public async Task<IActionResult> GetOtherRooms()
        {
            var ranking = await _roomsServices.GetRoomRanking();
            var highestRankingRoom = ranking.salles.OrderByDescending(s => int.Parse(s.WellnessValue)).FirstOrDefault();
            var otherRooms = ranking.salles.Where(s => s != highestRankingRoom).ToList();

            return Ok(otherRooms);
        }



    }
}
