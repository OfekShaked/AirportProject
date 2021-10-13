using AirportProject.BL.Models;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Moq;
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
        IMongoContext _context;
        IUnitOfWork _uow;
        DataAccess _dataAccess;
        public AirportTesting()
        {
            var appSettings = @"{""MongoSettings"":{
            ""Connection"" : ""mongodb://localhost:27017"",
            ""DatabaseName"" : ""AirportProjectTesting"",
            }}";
            Microsoft.Extensions.Configuration.IConfigurationBuilder builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.AddJsonStream(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
            var configuration = builder.Build();
            _context = new MongoContext(configuration);
            _uow = new UnitOfWork(_context);
            _dataAccess = new DataAccess(_context);
        }
        [Fact]
        public async void TestAirportMovings()
        {
            _context.DropTestDB();
            IAirport airport = new Airport(_dataAccess, _uow);
            Plane plane1 = new Plane() { Status = PlaneStatus.Arrival, Id = "1" };
            Plane plane2 = new Plane() { Status = PlaneStatus.Arrival, Id = "2" };
            Plane plane3 = new Plane() { Status = PlaneStatus.Departure, Id = "3" };
            Plane plane4 = new Plane() { Status = PlaneStatus.Arrival, Id = "4" };
            airport.GetPlaneFromSimulator(plane1);
            airport.GetPlaneFromSimulator(plane2);
            airport.GetPlaneFromSimulator(plane3);
            airport.GetPlaneFromSimulator(plane4);
            await Task.Delay(15000);
            int planesInStationsCount = 0;
            for (int i = 0; i < airport.Stations.Count; i++)
            {
                if (airport.Stations[i].CurrentPlaneInside != null)
                {
                    planesInStationsCount++;
                }
            }
            Assert.True(planesInStationsCount != 0);
        }
        [Fact]
        public async void TestStationIsEmpty()
        {
            //tests both moving station and and next is empty
            _context.DropTestDB();
            IAirport airport = new Airport(_dataAccess, _uow);
            airport.CreateNewAirport();
            Plane plane1 = new Plane() { Status = PlaneStatus.Arrival, Id = "1" };
            airport.GetPlaneFromSimulator(plane1);
            await Task.Delay(15000);
            Assert.True(airport.Stations[1].CurrentPlaneInside == null);
        }
    }
}
