using AirportProject.BL;
using AirportProject.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportProject.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeparturesController : Controller
    {
        GeneralLogic _logic;
        public DeparturesController(GeneralLogic logic)
        {
            _logic = logic;
        }
        [HttpGet]
        public async Task<List<DepartureDTO>> Get()
        {
            return await _logic.GetFutureDepartures();
        }
    }
}
