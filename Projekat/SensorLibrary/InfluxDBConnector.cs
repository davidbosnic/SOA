using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace SensorLibrary
{
    public class InfluxDBConnector : IInfluxDBConnector
    {

        public const string DB_CONNECTION_URL = "http://influxdb:8086";
        public const string DB_BUCKET = "soa";
        public const string DB_ORGANIZATION = "soa";
        public const string DB_USER = "admin";

        private static InfluxDBClient _client = null;

        private static InfluxDBConnector _connector = null;

        private static readonly object objLock = new object();

        private InfluxDBConnector()
        {
            CreateDatabase();
        }

        public static InfluxDBConnector GetInstance()
        {
            if (_connector == null)
            {
                lock (objLock)
                {
                    if (_connector == null)
                    {
                        _connector = new InfluxDBConnector();
                    }
                }
            }

            return _connector;
        }

        private static void CreateDatabase()
        {
            _client = InfluxDBClientFactory.Create(DB_CONNECTION_URL, DB_USER, "12345678".ToCharArray());
        }

        public void Write(string data)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WriteRecord(DB_BUCKET, DB_ORGANIZATION, InfluxDB.Client.Api.Domain.WritePrecision.Ms, data);
                Console.WriteLine("Data saved to influxdb");
            }
        }

        public void Write<T>(T data)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WriteMeasurement<T>(DB_BUCKET, DB_ORGANIZATION, InfluxDB.Client.Api.Domain.WritePrecision.Ms, data);
                Console.WriteLine("Data saved to influxdb");
            }
        }

        public void Write(PointData point)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WritePoint(DB_BUCKET, DB_ORGANIZATION, point);
                Console.WriteLine("Point saved to influxdb");
            }
        }

        public async Task<List<FluxTable>> Query(string query)
        {
            return await _client.GetQueryApi().QueryAsync(query, DB_ORGANIZATION);
        }
    }
}
