namespace MapApi.Repositories
{
    using MapApiCore.Interfaces;

    public class PollutionRepository : MarkerRepositoryBase, IPollutionRepository
    {
        protected override string DataFileName => "Pollution.json";
    }
}