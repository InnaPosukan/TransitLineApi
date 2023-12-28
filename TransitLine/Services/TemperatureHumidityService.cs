using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using TransitLine.DBContext;
using TransitLine.Interfaces;
using TransitLine.Models;

public class TemperatureHumidityService : ITemperatureHumidityService
{
    private readonly TransitLineContext _context;

    public TemperatureHumidityService(TransitLineContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IActionResult> AddSensor(int idDelivery, TemperatureHumidity sensor)
    {
        try
        {
            var delivery = await _context.Deliveries.FindAsync(idDelivery);

            if (delivery == null)
            {
                return new NotFoundObjectResult($"Delivery with ID {idDelivery} not found");
            }

            var temperatureHumidityReading = new TemperatureHumidity
            {
                Timestamp = DateTime.Now,
                Temperature = sensor.Temperature,
                Humidity = sensor.Humidity,
                IdDelivery = idDelivery
            };

            _context.TemperatureHumidities.Add(temperatureHumidityReading);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Sensor reading added successfully");
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"Error: {ex.Message}, InnerException: {ex.InnerException?.Message}");
        }
    }
    public async Task<IActionResult> UpdateSensor(int idDelivery, TemperatureHumidity updatedSensor)
    {
        try
        {
            var delivery = await _context.Deliveries.FindAsync(idDelivery);

            if (delivery == null)
            {
                return new NotFoundObjectResult($"Delivery with ID {idDelivery} not found");
            }

            var sensorToUpdate = await _context.TemperatureHumidities
                .Where(s => s.IdDelivery == idDelivery)
                .OrderByDescending(s => s.Timestamp)
                .FirstOrDefaultAsync();

            if (sensorToUpdate == null)
            {
                return new NotFoundObjectResult($"Sensor reading not found for Delivery ID {idDelivery}");
            }

            sensorToUpdate.Temperature = updatedSensor.Temperature;
            sensorToUpdate.Humidity = updatedSensor.Humidity;
            sensorToUpdate.Timestamp = DateTime.Now;

            await _context.SaveChangesAsync();

            return new OkObjectResult("Sensor reading updated successfully");
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"Error: {ex.Message}, InnerException: {ex.InnerException?.Message}");
        }
    }

}
