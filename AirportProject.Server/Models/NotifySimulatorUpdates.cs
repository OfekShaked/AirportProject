using AirportProject.BL.Interfaces;
using AirportProject.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportProject.Server.Models
{
    public class NotifySimulatorUpdates : INotifySimulatorUpdates
    {
        public Action<bool> NotifySimulatorToggled { get; set; }

        public NotifySimulatorUpdates(IHubContext<SimulatorHub> hub)
        {
            NotifySimulatorToggled = (bool isOn) => { hub.Clients.All.SendAsync("IsSimulatorRunning", isOn); };
        }
    }
}
