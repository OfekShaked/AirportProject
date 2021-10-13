using AirportProject.BL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirportProject.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimulatorController : Controller
    {
        GeneralLogic _logic;
        public SimulatorController(GeneralLogic logic)
        {
            _logic = logic;
        }
        [Route("Start")]
        [HttpGet]
        public bool Start()
        {
            try
            {
                _logic.StartSimulator();
                return true;
            }catch(Exception e)
            {
                e = e;
                return false;
            }
        }
        [Route("Stop")]
        [HttpGet]
        public bool Stop()
        {
            try { 
            _logic.StopSimulator();
            return true;
            }catch(Exception e)
            {
                e = e;
                return false;
            }
}
        [Route("IsRunning")]
        [HttpGet]
        public bool IsRunning()
        {
            return _logic.IsSimulatorRunning(); 
        }
    }
}
