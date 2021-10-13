using AirportProject.BL.Exceptions;
using AirportProject.BL.Models;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
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
        public Mock<IAirport> airport = new Mock<IAirport>();
        public IMongoContext _context ;
        public Mock<IUnitOfWork> _uow = new Mock<IUnitOfWork>();
        public IDataAccess _dataAccess;
        public StationTesting()
        {
            var appSettings = @"{""MongoSettings"":{
            ""Connection"" : ""mongodb://localhost:27017"",
            ""DatabaseName"" : ""AirportProjectTesting"",
            }}";
            Microsoft.Extensions.Configuration.IConfigurationBuilder builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.AddJsonStream(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
            var configuration = builder.Build();
            _context = new MongoContext(configuration);
            _dataAccess = new DataAccess(_context);
        }
        [Fact]
        public void TestFlightNotAcceptedWhenStationIsNotEmpty()
        {
            IStation station = new Station(1,airport.Object, _dataAccess,_uow.Object);
            station.AddPlaneToStation(new Plane());
            Assert.False(station.AddPlaneToStation(new Plane()));
        }
        [Fact]
        public void TestFlightReceivedWhenStationIsEmpty()
        {
            IStation station = new Station(1, airport.Object, _dataAccess, _uow.Object);
            station.AddPlaneToStation(new Plane());
            Assert.True(station.IsPlaneInside() != false);
        }
        [Fact]
        public void TestFreeStation()
        {
            IStation station = new Station(1, airport.Object, _dataAccess, _uow.Object);
            station.AddPlaneToStation(new Plane());
            Assert.True(station.IsPlaneInside() != false);
            station.SetStationFree();
            Assert.True(station.IsPlaneInside() == false);
        }
        [Fact]
        public void TestAddToQueueOnePlane()
        {
            IStation station = new Station(1, airport.Object, _dataAccess, _uow.Object);
            station.AddPlaneToStation(new Plane());
            station.AddPlaneToQueue(new Plane());
            Assert.True(station.PlanesWaitingForStation.Count == 1);
        }
        [Fact]
        public void TestRemovePlaneFromQueueOfOne()
        {
            IStation station = new Station(1, airport.Object, _dataAccess, _uow.Object);
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
            IStation station = new Station(1, airport.Object, _dataAccess, _uow.Object);
            station.AddPlaneToStation(new Plane());
            IPlane plane1ToRem = new Plane();
            IPlane plane2ToRem = new Plane();
            station.AddPlaneToQueue(plane1ToRem);
            station.AddPlaneToQueue(plane2ToRem);
            Assert.True(station.PlanesWaitingForStation.Count == 1);
            plane2ToRem.Id = "12";
            station.AddPlaneToQueue(plane2ToRem);
            Assert.True(station.PlanesWaitingForStation.Count == 2);
            IPlane plane1Waiting,plane2Waiting;
            plane1Waiting = station.PlanesWaitingForStation.Peek();
            Assert.True(plane1Waiting == plane1ToRem);
            station.RemovePlaneFromQueue(plane1ToRem);
            Assert.True(station.PlanesWaitingForStation.Count == 1);
            plane2Waiting = station.PlanesWaitingForStation.Peek();
            Assert.True(plane2Waiting == plane2ToRem);

        }
    }
}
