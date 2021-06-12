using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorCommandService.Models
{
    public class MessageHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
