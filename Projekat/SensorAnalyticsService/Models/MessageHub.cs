using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorAnalyticsService.Models
{
    public class MessageHub : Hub
    {
        public string GetConnectionId()
        {
            return "clientId-I4etnf7yfD";// Context.ConnectionId;
        }
    }
}
