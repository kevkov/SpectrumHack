namespace MapApiCore.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Models;

    public class JourneyRepository : RepositoryBase, IJourneyRepository
    {
        private const string fileName = "Journey.json";

        private readonly List<Journey> _journeys;

        public JourneyRepository()
        {
            _journeys = ReadData<Journey>(fileName);
        }

        public List<Route> GetRoutesForJourney(int journeyId)
        {
            var journey = this._journeys.FirstOrDefault(j => j.JourneyId == journeyId);

            return journey?.Routes;
        }
    }
}
