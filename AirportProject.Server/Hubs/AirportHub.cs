using AirportProject.DAL.Models;
using AirportProject.DAL.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportProject.Server.Hubs
{
    public class AirportHub : Hub
    {
        public Task UpdateAirport(AirportDTO airport)
        {
            return Clients.All.SendAsync("UpdateAirport", airport);
        }
        public Task AddArrival(ArrivalDTO arrival)
        {
            return Clients.All.SendAsync("AddArrival", arrival);
        }
        public Task RemoveArrival(string planeId)
        {
            return Clients.All.SendAsync("RemoveArrival", planeId);
        }
        public Task AddDeparture(DepartureDTO departure)
        {
            return Clients.All.SendAsync("AddDeparture", departure);
        }
        public Task RemoveDeparture(string planeId)
        {
            return Clients.All.SendAsync("RemoveDeparture", planeId);
        }
    }
}
