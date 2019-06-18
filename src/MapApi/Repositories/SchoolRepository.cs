namespace MapApi.Repositories
{
    using MapApiCore.Interfaces;

    public class SchoolRepository : MarkerRepositoryBase, ISchoolRepository
    {
        protected override string DataFileName => "School.json";
    }
}