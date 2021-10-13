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
    public class AirportController : Controller
    {
        GeneralLogic _logic;
        public AirportController(GeneralLogic logic)
        {
            _logic = logic;
        }
        [HttpGet]
        public AirportDTO Get()
        {
            return _logic.GetAirport();
        }
     }
}
