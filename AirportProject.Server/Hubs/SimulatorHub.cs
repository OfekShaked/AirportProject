using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportProject.Server.Hubs
{
    public class SimulatorHub : Hub
    {
        public Task ToggleSimulatorRunning(bool isRunning)
        {
            return Clients.All.SendAsync("IsSimulatorRunning", isRunning);
        }
    }
}
