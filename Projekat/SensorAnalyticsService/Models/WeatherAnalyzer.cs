using SensorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SensorAnalyticsService.Models
{
    public class WeatherAnalyzer
    {
        public Data Data { get; set; }

        public WeatherAnalyzer()
        {
            Data = null;
        }

        public bool Check()
        {
            return Data != null;
        }

        public void Clear()
        {
            Data = null;
        }


        public Data Analyze()
        {
            const double noWindPressure = 1040.0;
            const double windPressure = 1030.0;
            const double noRainfallCloudLevel = 0.5;

            if (Data.SensorType == "pressure")
            {
                if (Data.Value > noWindPressure)
                {
                    Data.Value = 0;
                    return Data;
                }
                else if (Data.Value < windPressure)
                {
                    Data.Value = 1;
                    return Data;
                }
            }
            if (Data.SensorType == "cloud")
            {
                if (Data.Value <= noRainfallCloudLevel)
                {
                    Data.Value = 0;
                    return Data;
                }
                Data.Value = 1;
                return Data;
            }

            return null;//no command
        }
    }
}
