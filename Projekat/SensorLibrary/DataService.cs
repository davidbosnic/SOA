using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using SensorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;
using static uPLibrary.Networking.M2Mqtt.MqttClient;

namespace SensorDataService.Service
{
    public class DataService
    {
        private readonly Mqtt _mqtt;
        private readonly IInfluxDBConnector _database;

        private event EventHandler ServiceCreated;
        public DataService()
        {
            _mqtt = new Mqtt();
            _database = InfluxDBConnector.GetInstance();

            ServiceCreated += OnServiceCreated;
            ServiceCreated?.Invoke(this, EventArgs.Empty);
        }

        private void OnServiceCreated(object sender, EventArgs args)
        {
            if (_mqtt.IsConnected())
            {
                _mqtt.Subscribe("sensor/data", OnDataReceived);
                Console.WriteLine("Subscribed");
            }
        }

        private void OnDataReceived(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var json_data = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine(json_data);
                Data sensorData = JsonConvert.DeserializeObject<Data>(json_data);
                if (_mqtt.IsConnected())
                    _mqtt.Publish(sensorData, "data-analytics/data");
                this.SaveData(sensorData);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void PublishAsync(object data, string topic)
        {
            _mqtt.Publish(data, topic);
        }

        public void SaveData(Data sensorData)
        {
            var point = PointData
                      .Measurement("SensorsData")
                      .Tag("sensor", sensorData.SensorType.ToLower())
                      .Field("value", sensorData.Value)
                      .Timestamp(DateTime.UtcNow, WritePrecision.Ms);
            _database.Write(point);
            //Console.WriteLine("added to db");
        }
    }
}
