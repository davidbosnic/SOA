using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using SensorLibrary;
using SensorAnalyticsService.Models;
using RabbitMQ.Client.Events;

namespace SensorAnalyticsService.Services
{
    public class AnalyticsService
    {
        private readonly Mqtt _mqtt;
        private readonly IInfluxDBConnector _database;
        private event EventHandler ServiceCreated;
        private readonly WeatherAnalyzer _model;
        //private IHubContext<MessageHub> _hubContext;
        public AnalyticsService()
        {
            _mqtt = new Mqtt();
            _database = InfluxDBConnector.GetInstance();
            //_hubContext = null;
            _model = new WeatherAnalyzer();
            ServiceCreated += OnServiceCreated;
            ServiceCreated?.Invoke(this, EventArgs.Empty);
        }

        private void OnServiceCreated(object sender, EventArgs args)
        {
            try
            {
                //if (_mqtt.IsConnected())
                //{
                    _mqtt.Subscribe("sensor/data", OnDataReceived);
                    Console.WriteLine("subscribed");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OnDataReceived(object sender, BasicDeliverEventArgs arg)
        {
            try
            {
                Data data = JsonConvert.DeserializeObject<Data>(
                    Encoding.UTF8.GetString(arg.Body.ToArray()));

                Console.WriteLine($"type: {data.SensorType}, val: {data.Value}");

                _model.Data = data;
                if (!_model.Check()) return;
                var point = PointData
                        .Measurement("AnalyticsData")
                        .Field(data.SensorType, data.Value)
                        .Timestamp(DateTime.UtcNow, WritePrecision.Ms);
                _database.Write(point);
                Data result = _model.Analyze();
                SendActionCommandMicroservice(result);
                //SendEventToWebDashboard(eventVal);
                _model.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SendActionCommandMicroservice(Data command)
        {
            try
            {
                _mqtt.Publish(command, "sensor/analyze");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private async void SendEventToWebDashboard(string eventVal)
        {
            //await _hubContext.Clients.All.SendAsync("SendEvent", eventVal);
        }
    }
}
