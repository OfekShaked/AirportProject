using AirportProject.BL.Exceptions;
using AirportProject.BL.Models;
using AirportProject.Common.Interfaces;
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
            IStation station = new Station(1, airport);
            station.AddPlaneToStation(new Plane());
            Assert.True(station.CurrentPlaneInside != null);
        }
        [Fact]
        public void TestFreeStation()
        {
            IStation station = new Station(1, airport);
            station.AddPlaneToStation(new Plane());
            Assert.True(station.CurrentPlaneInside != null);
            station.SetStationFree();
            Assert.True(station.CurrentPlaneInside == null);
        }
        [Fact]
        public void TestAddToQueueOnePlane()
        {
            IStation station = new Station(1, airport);
            station.AddPlaneToStation(new Plane());
            station.AddPlaneToQueue(new Plane());
            Assert.True(station.PlanesWaitingForStation.Count == 1);
        }
        [Fact]
        public void TestRemovePlaneFromQueueOfOne()
        {
            IStation station = new Station(1, airport);
            station.AddPlaneToStation(new Plane());
            IPlane planeToRem = new Plane();
            station.AddPlaneToQueue(planeToRem);
            Assert.True(station.PlanesWaitingForStation.Count == 1);
            station.RemovePlaneFromQueue(planeToRem);
            Assert.True(station.PlanesWaitingForStation.Count == 0);
        }
        [Fact]
        public void TestRemovePlanyFromQueueOfTwo()
        {
            IStation station = new Station(1, airport);
            station.AddPlaneToStation(new Plane());
            IPlane plane1ToRem = new Plane();
            IPlane plane2ToRem = new Plane();
            station.AddPlaneToQueue(plane1ToRem);
            station.AddPlaneToQueue(plane2ToRem);
            Assert.True(station.PlanesWaitingForStation.Count == 2);
            Assert.True(station.PlanesWaitingForStation.Peek()==plane1ToRem);
            station.RemovePlaneFromQueue(plane1ToRem);
            Assert.True(station.PlanesWaitingForStation.Count == 1);
            Assert.True(station.PlanesWaitingForStation.Peek() == plane2ToRem);

        }
    }
}
