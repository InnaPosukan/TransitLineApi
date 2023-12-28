using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitLine.DBContext;
using TransitLine.Interfaces;
using TransitLine.Models;
using System;
using System.Threading.Tasks;
using TransitLine.Services;

namespace TransitLine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureHumidityController : ControllerBase
    {
        private readonly ITemperatureHumidityService _temperatureHumidityService;

        public TemperatureHumidityController(ITemperatureHumidityService temperatureHumidityService)
        {
            _temperatureHumidityService = temperatureHumidityService ?? throw new ArgumentNullException(nameof(temperatureHumidityService));
        }

        [HttpPost("AddSensor/{idDelivery}")]
        public async Task<IActionResult> AddSensorReading(int idDelivery, [FromBody] TemperatureHumidity sensor)
        {
            return await _temperatureHumidityService.AddSensor(idDelivery, sensor);
        }
        [HttpPut("UpdateSensor/{idDelivery}")]
        public async Task<IActionResult> UpdateSensor(int idDelivery, [FromBody] TemperatureHumidity updatedSensor)
        {
            return await _temperatureHumidityService.UpdateSensor(idDelivery, updatedSensor);
        }
    }
}
