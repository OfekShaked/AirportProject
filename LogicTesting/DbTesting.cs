using AirportProject.BL.Models;
using AirportProject.Commom.Enums;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Moq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LogicTesting
{
    public class DbTesting
    {
        IMongoContext _context;
        IUnitOfWork _uow;
        DataAccess _dataAccess;
        public DbTesting()
        {
            var appSettings = @"{""MongoSettings"":{
            ""Connection"" : ""mongodb://localhost:27017"",
            ""DatabaseName"" : ""AirportProjectTesting"",
            }}";
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
            var configuration = builder.Build();
            _context = new MongoContext(configuration);
            _uow = new UnitOfWork(_context);
            _dataAccess = new DataAccess(_context);
        }
        [Fact]
        public async void TestAddToDB()
        {
            _context.DropTestDB();
            IAirport airport = new Airport(_dataAccess, _uow);
            var planes = await _dataAccess.PlaneRepository.GetAll();
            Plane plane1 = new Plane() {Status = PlaneStatus.Arrival, Id="1" };
            Plane plane2 = new Plane() { Status = PlaneStatus.Arrival, Id = "2" };
            Plane plane3 = new Plane() { Status = PlaneStatus.Departure, Id = "3" };
            Plane plane4 = new Plane() { Status = PlaneStatus.Arrival, Id = "4" };
            airport.GetPlaneFromSimulator(plane1);
            airport.GetPlaneFromSimulator(plane2);
            airport.GetPlaneFromSimulator(plane3);
            airport.GetPlaneFromSimulator(plane4);
            await Task.Delay(15000);
            var planesUpdated = await _dataAccess.PlaneRepository.GetAll();
            Assert.True(planesUpdated.Count()- planes.Count()==4);
        }
    }
}
