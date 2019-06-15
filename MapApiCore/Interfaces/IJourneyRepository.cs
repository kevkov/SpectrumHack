namespace MapApiCore.Interfaces
{
    using Models;

    public interface IJourneyRepository
    {
        Journey GetJourney(int journeyId);
    }
}
