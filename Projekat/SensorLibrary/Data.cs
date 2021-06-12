using System;
using System.Collections.Generic;
using System.Text;

namespace SensorLibrary
{
    public class Data
    {
        public string SensorType { get; set; }
        public double Value { get; set; }

        public Data(double value, string sensorType)
        {
            this.Value = value;
            this.SensorType = sensorType;
        }
    }
}
