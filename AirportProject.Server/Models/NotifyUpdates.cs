using AirportProject.BL.Interfaces;
using AirportProject.DAL.Models;
using AirportProject.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportProject.Server.Models
{
    public class NotifyUpdates : INotifyUpdates
    {
        public Action<AirportDTO> UpdateAirport { get; set; }
        public Action<ArrivalDTO> AddArrival { get; set; }
        public Action<string> RemoveArrival { get; set; }
        public Action<DepartureDTO> AddDeparture { get; set; }
        public Action<string> RemoveDeparture { get; set; }
        public Action<List<DepartureDTO>> UpdatePlannedDepartures { get; set; }
        public Action<List<ArrivalDTO>> UpdatePlannedArrivals { get; set; }

        public NotifyUpdates(IHubContext<AirportHub> hub)
        {
            UpdateAirport = (AirportDTO airport) => Task.Run(async () => { await hub.Clients.All.SendAsync("UpdateAirport", airport);  });
            AddArrival = (ArrivalDTO arrival) => Task.Run(async () => { await hub.Clients.All.SendAsync("AddArrival", arrival);  });
            RemoveArrival = (string planeId) => Task.Run(async () => { await hub.Clients.All.SendAsync("RemoveArrival", planeId); });
            AddDeparture = (DepartureDTO departure) => Task.Run(async () => { await hub.Clients.All.SendAsync("AddDeparture", departure); });
            RemoveDeparture = (string planeId) => Task.Run(async () => { await hub.Clients.All.SendAsync("RemoveDeparture", planeId); });
            UpdatePlannedDepartures = (List<DepartureDTO> departures) => Task.Run(async () => { await hub.Clients.All.SendAsync("UpdateDepartures", departures); });
            UpdatePlannedArrivals = (List<ArrivalDTO> arrivals) => Task.Run(async () => { await hub.Clients.All.SendAsync("UpdateArrivals", arrivals); });
        }
    }
}
