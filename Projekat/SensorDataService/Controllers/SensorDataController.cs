using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SensorDataService.Model;
using SensorDataService.DBContext;
using SensorDataService.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SensorDataService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SensorDataController : Controller
    {
        private IWeatherRepository _repository;

        public SensorDataController()
        {
            _repository = new WeatherRepository(WeatherContext.GetInstance());
        }

        [HttpPost]
        public async Task<IActionResult> AddSensorData([FromBody, Required] SensorDataModel sensorDataModel)
        {
            if (sensorDataModel == null)
            {
                return BadRequest();
            }
            await _repository.AddSensorDataAsync(sensorDataModel);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSensorData()
        {
            return Ok(await _repository.GetAllSensorDataAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypedTresholdSensorData([Required]string tresholdOfSensor, [Required]string typeOfSensor)
        {
            if (tresholdOfSensor == null || typeOfSensor == null)
            {
                return BadRequest();
            }
            return Ok(await _repository.GetAllTypedTresholdSensorDataAsync(typeOfSensor, tresholdOfSensor));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypedSensorData([Required] string typeOfSensor)
        {
            if(typeOfSensor == null)
            {
                return BadRequest();
            }
            return Ok(await _repository.GetAllTypedSensorDataAsync(typeOfSensor));
        }

        [HttpPut]
        public async Task<IActionResult> ModifySensorData([Required, FromBody]SensorDataModel sdm)
        {
            if (sdm == null)
            {
                return BadRequest();
            }
            await _repository.ModifySensorDataAsync(sdm);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAllData()
        {
            await _repository.RemoveAllDataAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveSensorData([Required] string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            await _repository.RemoveSensorDataAsync(id);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTypedSensorData([Required] string typeOfSensor)
        {
            if (typeOfSensor == null)
            {
                return BadRequest();
            }
            await _repository.RemoveTypedSensorDataAsync(typeOfSensor);
            return Ok();
        }

    }
}
