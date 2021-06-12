using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensorLibrary
{
    public interface IInfluxDBConnector
    {
        void Write(string data);
        void Write(PointData data);
        void Write<T>(T data);
        Task<List<FluxTable>> Query(string query);
    }
}
