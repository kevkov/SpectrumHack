using MapApiCore.Models;
using MapApiCore.Repositories;
using MapApiDataFeeder.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace MapApiDataFeeder.Services
{
    class AirPollutionService : IAirPollutionService
    {
        private const string apiKey = "zZTKfHCg9CnMEcKvk";
        private readonly IPollutionRepository _pollutionRepository;
        private string _jsonFilePath;

        public AirPollutionService(IPollutionRepository pollutionRepository, string jsonFilePath)
        {
            _pollutionRepository = pollutionRepository;
            _jsonFilePath = jsonFilePath;
        }

        public async Task AirPollutionByCoordinateAsync(Coordinate coordinate)
        {
            var apiPath = $"/nearest_city?lat={coordinate.Longitude}&lon={coordinate.Latitude}&key={apiKey}";

            var response = await _pollutionRepository.MarkersAsync(apiPath);

            if (response != null)
            {
                var jsonString = JsonConvert.SerializeObject(response);
                using (var streamWriter = new StreamWriter(_jsonFilePath))
                {
                    await streamWriter.WriteLineAsync(jsonString);
                    streamWriter.Close();
                }
            }
        }
    }
}
