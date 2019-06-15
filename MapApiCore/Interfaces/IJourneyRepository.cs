namespace MapApiCore.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IJourneyRepository
    {
        List<Route> GetRoutesForJourney(int journeyId);
    }
}
