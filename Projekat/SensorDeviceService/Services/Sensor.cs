using SensorLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SensorDeviceService.Services
{
    public class Sensor
    {
        private readonly Timer _timer;
        private readonly StreamReader _streamReader;

        public double Value { get; set; }
        public string SensorType { get; set; }
        public double TimerInterval { get; set; }
        public double TresholdValue { get; set; }
        public bool IsTresholdMeasuring { get; set; }


        public Sensor(string type, double treshold, string path)
        {
            SensorType = type;
            TresholdValue = treshold;
            IsTresholdMeasuring = false;
            TimerInterval = 3000;
            _streamReader = new StreamReader(path);
            _timer = new Timer(TimerInterval);
            _timer.Elapsed += TimePased;
        }


        public void SetTimeout(double interval)
        {
            StopSensor();
            TimerInterval = interval;
            _timer.Interval = interval;
            StartSensor();
        }

        public void StopSensor()
        {
            if(_timer.Enabled)
                _timer.Stop();
        }
        public void StartSensor()
        {
            if(!_timer.Enabled)
                _timer.Start();
        }

        public void SetTresholdValue(double treshold)
        {
            IsTresholdMeasuring = true;
            TresholdValue = treshold;
        }

        private async void TimePased(object sender, ElapsedEventArgs e)
        {
            //_mqtt.Connect();
            try
            {
                ReadData();
                //Data data = new Data(this.Value, this.SensorType);
                //if (!this.IsTresholdMeasuring)
                //{
                //    _mqtt.Publish(data, "sensor/data");
                //    Console.WriteLine($"{data.SensorType}: {data.Value}");
                //}
                //else if (data.Value > this.TresholdValue)
                //    _mqtt.Publish(data, "sensor/data");
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            await SendValueAsync();
            //obrati paznju
        }

        private async Task SendValueAsync()
        {
            if(!IsTresholdMeasuring || (IsTresholdMeasuring && (Value > TresholdValue)))
                await PostRequest("http://sensordataservice/api/SensorData/AddSensorData");
        }

        private async Task PostRequest(string url)
        {

            Console.WriteLine(DateTime.Now.ToShortTimeString());
            Console.WriteLine(SensorType);
            Console.WriteLine(Value);

            try
            {
                HttpClient httpClient = new HttpClient();
                await httpClient.PostAsync(url, new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        recordTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                        type = SensorType,
                        value = Value.ToString(),
                        id = "",
                    }
                ), Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ReadData()
        {
            try
            {
                string line;
                if (!_streamReader.EndOfStream)
                    line = _streamReader.ReadLine();
                else
                {
                    _streamReader.DiscardBufferedData();
                    _streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    line = _streamReader.ReadLine();
                }
                Value = double.Parse(line, CultureInfo.InvariantCulture);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
