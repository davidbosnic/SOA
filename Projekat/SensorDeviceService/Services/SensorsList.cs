using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorDeviceService.Services
{
    public class SensorsList
    {
        private static SensorsList sensors_list = null;
        private static readonly object objLock = new object();
        private readonly List<Sensor> _sensors;

        private SensorsList()
        {
            _sensors = new List<Sensor>
            {
                new Sensor("temperature", 25, "./Resources/temperature.txt"),
                new Sensor("humidity", 50, "./Resources/humidity.txt"),
                new Sensor("rainfall", 0.5, "./Resources/rainfall.txt"),
                new Sensor("pressure", 1013, "./Resources/pressure.txt"),
                new Sensor("windspeed", 15, "./Resources/windspeed.txt"),
                new Sensor("cloud", 10, "./Resources/cloud.txt")
            };
        }

        public List<Sensor> GetSensors()
        {
            if (sensors_list == null)
                sensors_list = new SensorsList();
            return _sensors;
        }

        public static SensorsList GetSensorsListInstance()
        {
            if (sensors_list == null)
            {
                lock (objLock)
                {
                    if (sensors_list == null)
                    {
                        sensors_list = new SensorsList();
                    }
                }
            }

            return sensors_list;
        }
    }
}
