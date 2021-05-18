using SensorDataService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorDataService.Repository
{
    public interface IWeatherRepository
    {

        //odavde je novo
        Task AddSensorDataAsync(SensorDataModel sdm);

        Task<IList<SensorDataModel>> GetAllSensorDataAsync();

        Task ModifySensorDataAsync(SensorDataModel sdm);

        Task RemoveSensorDataAsync(string id);

        Task RemoveAllDataAsync();

        Task RemoveTypedSensorDataAsync(string type);

        Task<IList<SensorDataModel>> GetAllTypedSensorDataAsync(string type);

        Task<IList<SensorDataModel>> GetAllTypedTresholdSensorDataAsync(string type, string treshold);

    }
}
