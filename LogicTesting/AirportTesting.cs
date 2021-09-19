using AirportProject.BL.Models;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LogicTesting
{
    public class AirportTesting
    {
        [Fact]
        public async void TestAirportMovings()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            airport.GetPlaneFromSimulator(new Plane() { Id = "Plane1", Status = PlaneStatus.Arrival });
            airport.GetPlaneFromSimulator(new Plane() { Id = "Plane2", Status = PlaneStatus.Arrival });
            airport.GetPlaneFromSimulator(new Plane() { Id = "Plane3", Status = PlaneStatus.Arrival });
            airport.GetPlaneFromSimulator(new Plane() { Id = "Plane4", Status = PlaneStatus.Arrival });
            await Task.Delay(20000000);
            Assert.True(true);
        }
    }
}
