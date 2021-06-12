using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SensorAnalyticsService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SensorAnalyticsService.Services;
using System.Net.Http;

namespace SensorAnalyticsService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SensorAnalyticsController : Controller
    {

        private readonly IHubContext<MessageHub> _hubContext;
        private AnalyticsService _analyticsService;

        public SensorAnalyticsController(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
            HttpClient httpClient = new HttpClient();
            httpClient.PostAsync("http://sensorcommandservice/api/SensorCommand/CreateCommandService", null);
        }

        [HttpPost]
        public IActionResult CreateAnalyticsService()
        {
            if (_analyticsService == null)
                _analyticsService = new AnalyticsService();
            return Ok();
        }

        [HttpPost("{type}")]
        public async Task<IActionResult> Subscribe(
            [Required, FromQuery(Name = "connectionId")] string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, "analytics");
            return Ok();
        }
        
    }
}
