using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SensorCommandService.Models;
using SensorCommandService.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SensorCommandService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SensorCommandController : Controller
    {
        private readonly CommandService _commandService;
        private bool onoff;
        private IHubContext<MessageHub> _hubContext;

        public SensorCommandController(IHubContext<MessageHub> hubContext)
        {
            _commandService = new CommandService(hubContext);
            _hubContext = hubContext;
            onoff = true;
        }

        [HttpPost]
        public IActionResult CreateCommandService()
        {
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Subscribe(
            [Required, FromQuery(Name = "connectionId")] string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, "analytics");
            return Ok();
        }
    }
}
