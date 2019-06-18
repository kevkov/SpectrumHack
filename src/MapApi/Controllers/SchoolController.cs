namespace MapApi.Controllers
{
    using System.Collections.Generic;
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolRepository _schoolRepository;
        
        public SchoolController(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        // GET api/School
        [HttpGet]
        public ActionResult<List<Marker>> Get()
        {
            var markers = _schoolRepository.GetMarkers();

            return markers;
        }
    }
}