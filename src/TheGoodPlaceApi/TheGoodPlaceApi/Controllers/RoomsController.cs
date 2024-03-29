﻿using TheGoodPlaceApi.Dto;
using TheGoodPlaceApi.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(ranking);
        }
    }
}
