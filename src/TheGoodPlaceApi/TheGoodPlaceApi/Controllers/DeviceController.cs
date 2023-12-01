using Microsoft.AspNetCore.Mvc;
using TheGoodPlaceApi.Services;
using TheGoodPlaceApi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheGoodPlaceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly TableStorageService _mxChipService;

    public DeviceController (TableStorageService mxChipService)
    {
        _mxChipService = mxChipService;
    }
    [HttpGet]
    [Produces("application/json", Type = null)]
    public IActionResult Get()
    {
        return Ok(_mxChipService.GetAllDatas());
    }

    [HttpGet("LastRecordTemperatureHumidtyPressure")]
    [Produces("application/json", Type = null)]
    public IActionResult GetLastRecordTemperatureHumidtyPressure()
    {
        var latestHumidityDataForEachDevice = _mxChipService.GetAllDatasWithHumidity();

        return Ok(latestHumidityDataForEachDevice);
    }




}



