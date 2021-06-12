using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using SensorCommandService.Models;
using SensorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SensorCommandService.Service
{
    public class CommandService
    {
        private readonly Mqtt _mqtt;
        private IHubContext<MessageHub> _hubContext;


        public CommandService(IHubContext<MessageHub> hubContext)
        {
            _mqtt = new Mqtt();
            _hubContext = hubContext;
            _mqtt.Subscribe("sensor/analyze", OnReceive);

        }

        public void Start(string topic)
        {
            _mqtt.Subscribe(topic, OnReceive);
        }

        public void Stop(string topic)
        {
            _mqtt.UnSubscribe(topic);
        }

        private async void OnReceive(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                Data data = JsonConvert.DeserializeObject<Data>(
                    Encoding.UTF8.GetString(e.Body.ToArray()));
                if (data == null)
                    return;
                Console.WriteLine($"type: {data.SensorType}, val: {data.Value}");
                SendEventToWebDashboard("Sensor " + data.SensorType + " is turned " + ((data.Value == 1) ? "true" : "false"));
                HttpClient httpClient = new HttpClient();
                var responseMessage = await httpClient.PostAsync("http://sensordeviceservice/api/SensorDevice/TurnOnOffSensor?on=" + ((data.Value == 1) ? "true" : "false") + "&type=" +
                    data.SensorType, new StringContent(string.Empty, Encoding.UTF8, "application/json"));
                Console.WriteLine($"post response: {responseMessage}");

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private async void SendEventToWebDashboard(string eventVal)
        {
            await _hubContext.Clients.All.SendAsync("SendEvent", eventVal);
        }
    }
}
