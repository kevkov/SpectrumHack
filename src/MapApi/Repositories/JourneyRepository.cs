namespace MapApi.Repositories
{
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class JourneyRepository : RepositoryBase, IJourneyRepository
    {
        private const string fileName = "Journey.json";

        private readonly List<Journey> _journeys;

        public JourneyRepository()
        {
            this._journeys = ReadData<Journey>(fileName);
        }

        public void WriteJourney(List<Journey> journeys)
        {
            WriteData<Journey>(fileName, journeys);
        }

        public Journey GetJourney(int journeyId)
        {
            return this._journeys.FirstOrDefault(j => j.JourneyId == journeyId);
        }
    }
}
