using Microsoft.AspNetCore.Mvc;
using SensorDeviceService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SensorDeviceService.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SensorDeviceController : ControllerBase
    {
        private readonly SensorsList _sensorsList;

        public SensorDeviceController()
        {
            _sensorsList = SensorsList.GetSensorsListInstance();
        }


        [HttpPost]
        public IActionResult TurnOnOffSensor([Required] bool on, [Required] string type)
        {
            foreach (Sensor sensor in _sensorsList.GetSensors())
            {
                if (type == sensor.SensorType)
                {
                    string state;
                    if (on)
                    { 
                        sensor.StartSensor();
                        state = "on";
                    }
                    else
                    {
                        sensor.StopSensor();
                        state = "off";
                    }
                    return Ok($"sensor {type} turned {state}");
                }
            }
            return BadRequest("SensorType doesn't exist");
        }


        [HttpGet]
        public IActionResult GetThreshold([Required] string type)
        {
            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type == sensor.SensorType)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };
                    string tresholdInfo = JsonSerializer.Serialize(new { isTreshold = sensor.IsTresholdMeasuring, value = sensor.TresholdValue }, options);
                    return Ok(tresholdInfo);
                }
            }
            return BadRequest("SensorType of sensor doesn't exist");
        }


        [HttpPost]
        public virtual IActionResult SetThreshold([Required] string type, double? value)
        {
            if (value == null) 
                return BadRequest("Not valide treshold value");

            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type == sensor.SensorType)
                {
                    string measure = "default";
                    sensor.IsTresholdMeasuring = true;
                    if (value != null)
                    {
                        sensor.SetTresholdValue((double)value);
                        measure = $"{value}";
                    }
                    return Ok($"Treshold value for {type} sensor: {measure}");
                }
            }
            return BadRequest("SensorType doesn't exist");
        }


        [HttpGet]
        public virtual IActionResult GetTimeout([Required] string type)
        {
            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type == sensor.SensorType)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };

                    string timeoutInfo = JsonSerializer.Serialize(new
                    {
                        isTimeout = !sensor.IsTresholdMeasuring,
                        value = sensor.TimerInterval
                    }, options);

                    return Ok(timeoutInfo);
                }
            }
            return BadRequest("SensorType doesn't exist");
        }


        [HttpPost]
        public virtual IActionResult SetTimeout([Required] string type, double? value)
        {
            foreach (var sensor in this._sensorsList.GetSensors())
            {
                if (type == sensor.SensorType)
                {
                    string timeout = "default";
                    sensor.IsTresholdMeasuring = false;
                    if (value != null)
                    {
                        sensor.SetTimeout((double)value);
                        timeout= $"{value}";
                    }
                    return Ok($"Timeout started for {type} sensor. Timeout value: {timeout}");
                }
            }
            return BadRequest("SensorType doesn't exist");
        }


        [HttpGet]
        public virtual IActionResult GetSensorsMeatadata(string type)
        {
            List<Sensor> sendList = new List<Sensor>();
            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type == sensor.SensorType)
                    sendList.Add(sensor);
            }
            if (sendList.Count == 0)
                return BadRequest("SensorType doesn't exist");
            return Ok(sendList);
        }
    }
    
}
