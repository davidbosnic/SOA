using MongoDB.Driver;
using SensorDataService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorDataService.Repository
{
    public class WeatherRepository : IWeatherRepository
    {

        private readonly IMongoDatabase _dbContext;

        public WeatherRepository(IMongoDatabase db)
        {
            _dbContext = db;
        }

        public async Task AddSensorDataAsync(SensorDataModel sdm)
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            await coll.InsertOneAsync(sdm);
        }

        public async Task<IList<SensorDataModel>> GetAllSensorDataAsync()
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            return await coll.Find(x => true).ToListAsync();
        }

        public async Task<IList<SensorDataModel>> GetAllTypedSensorDataAsync(string type)
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            return await coll.Find(x => x.Type == type).ToListAsync();
        }

        public async Task<IList<SensorDataModel>> GetAllTypedTresholdSensorDataAsync(string type, string treshold)
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            return await coll.Find(x => x.Type == type && Convert.ToDouble(x.Value) >= Convert.ToDouble(treshold)).ToListAsync();
        }

        public async Task ModifySensorDataAsync(SensorDataModel sdm)
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            await coll.ReplaceOneAsync(x => x.ID == sdm.ID, sdm);
        }

        public async Task RemoveAllDataAsync()
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            await coll.DeleteManyAsync(x => true);
        }

        public async Task RemoveSensorDataAsync(string id)
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            await coll.DeleteOneAsync(x => x.ID==id);
        }

        public async Task RemoveTypedSensorDataAsync(string type)
        {
            var coll = _dbContext.GetCollection<SensorDataModel>("SENSORS");
            await coll.DeleteManyAsync(x => x.Type == type);
        }
    }
}
