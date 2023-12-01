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
            OpenAiService os = new OpenAiService("4a5a265069944ae487797615e7256df8", "https://openaideepwork.openai.azure.com/openai/deployments/deepwork/chat/completions?api-version=2023-07-01-preview");

            var ranking = await os.GetRoomRanking("j'ai une liste de salle: [Chambre:34°,Bureau:10°,Gallery:20°] et je souhaite les mettre dans l'ordre par rapport à l'heure wellnessvalue");
            //var ranking = await _roomsServices.GetRoomRanking();
            return Ok(ranking);
        }
    }
}
