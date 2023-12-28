using Microsoft.AspNetCore.Mvc;
using TransitLine.Models;

namespace TransitLine.Interfaces
{
    public interface ITemperatureHumidityService
    {
        Task<IActionResult> AddSensor(int idDelivery, TemperatureHumidity sensor);
        Task<IActionResult> UpdateSensor(int idDelivery, TemperatureHumidity updatedSensor);

    }

}
