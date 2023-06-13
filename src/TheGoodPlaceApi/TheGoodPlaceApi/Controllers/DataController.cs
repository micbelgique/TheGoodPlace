using TheGoodPlaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace TheGoodPlaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly DataService _service;

        public DataController(DataService service)
        {
            _service = service; 
        }

        [HttpGet]
        [Produces("application/json", Type = null)]
        public IActionResult Get()
        {
            return Ok(_service.GetAllDatas());
        }

        [HttpGet("{devEUI}")]
        [Produces("application/json", Type = null)]
        public IActionResult Get(string devEUI)
        {
            return Ok(_service.GetAllDatasByDevice(devEUI));
        }

        [HttpGet("{devEUI}/{container}")]
        [Produces("application/json", Type = null)]
        public IActionResult Get(string devEUI, string container)
        {
            return Ok(_service.GetAllDatasByDeviceAndContainer(devEUI, container));
        }

        [HttpGet("lastRecord/{devEUI}/{container}")]
        [Produces("application/json", Type = null)]
        public IActionResult GetLastRecord(string devEUI, string container)
        {
            return Ok(_service.Lastrecord(devEUI, container));
        }

        [HttpGet("lastRecords/{devEUI}/")]
        [Produces("application/json", Type = null)]
        public IActionResult GetLastRecords(string devEUI)
        {
            return Ok(_service.LastrecordsFromSpecificDevice(devEUI));
        }
    }
}
