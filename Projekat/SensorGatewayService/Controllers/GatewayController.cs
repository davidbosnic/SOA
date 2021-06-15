using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SensorGatewayService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GatewayController : Controller
    {
        private HttpClient _httpClient;

        public GatewayController()
        {
            this._httpClient = new HttpClient();
        }

        private async Task<ContentResult> ProxyPost(string url, string jsonBody)
        {
            var responseMessage = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            return Content(await responseMessage.Content.ReadAsStringAsync());
        }

        private async Task<ContentResult> ProxyGet(string url)
        {
            var responseMessage = await _httpClient.GetAsync(url);
            return Content(await responseMessage.Content.ReadAsStringAsync());
        }
            //=> Content(await _httpClient.GetStringAsync(url));

        private async Task<ContentResult> ProxyDelete(string url)
        {
            var responseMessage = await _httpClient.DeleteAsync(url);
            return Content(await responseMessage.Content.ReadAsStringAsync());
        }
        //SensorDataService

        [HttpGet]
        public async Task<IActionResult> GetAllSensorData()
            => await ProxyGet("http://sensordataservice/api/SensorData/GetAllSensorData");

        [HttpGet]
        public async Task<IActionResult> GetAllTypedSensorData([Required] string typeOfSensor)
            => await ProxyGet("http://sensordataservice/api/SensorData/GetAllTypedSensorData?typeOfSensor="+typeOfSensor);

        [HttpGet]
        public async Task<IActionResult> GetAllTypedTresholdSensorData([Required] string tresholdOfSensor, [Required] string typeOfSensor)
            => await ProxyGet("http://sensordataservice/api/SensorData/GetAllTypedTresholdSensorData?tresholdOfSensor=" + tresholdOfSensor + "&typeOfSensor=" + typeOfSensor);
        
        [HttpDelete]
        public async Task<IActionResult> RemoveAllData()
            => await ProxyDelete("http://sensordataservice/api/SensorData/RemoveAllData");

       
    }
}
