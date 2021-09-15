using AirportProject.BL.Exceptions;
using AirportProject.BL.Interfaces;
using AirportProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LogicTesting
{
    public class StationTesting
    {
        IAirport airport;
        public StationTesting()
        {
            airport = new Airport();
        }
        [Fact]
        public void TestFlightNotAcceptedWhenStationIsNotEmpty()
        {
            IStation station = new Station(1,airport);
            station.AddPlaneToStation(new Plane());
            Assert.Throws<StationException>(() => station.AddPlaneToStation(new Plane()));
        }
        [Fact]
        public void TestFlightReceivedWhenStationIsEmpty()
        {
            IAirport airport = new Airport();
            IStation station = new Station(1, airport);
            station.AddPlaneToStation(new Plane());
            Assert.True(station.CurrentPlaneInside != null);
        }
    }
}
