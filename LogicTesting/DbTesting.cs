using AirportProject.BL.Models;
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
        public async void TestAirportMovings()
        {
            IAirport airport = new Airport(_dataAccess, _uow);
            Plane plane1 = new Plane() {Status = PlaneStatus.Arrival };
            Plane plane2 = new Plane() { Status = PlaneStatus.Arrival };
            Plane plane3 = new Plane() { Status = PlaneStatus.Departure };
            Plane plane4 = new Plane() { Status = PlaneStatus.Arrival };
            airport.GetPlaneFromSimulator(plane1);
            airport.GetPlaneFromSimulator(plane2);
            airport.GetPlaneFromSimulator(plane3);
            airport.GetPlaneFromSimulator(plane4);
            await Task.Delay(30000);
            var data1 = await _dataAccess.PlaneRepository.GetById(plane1.Id);
            Assert.True(data1.Status==PlaneStatus.Finished);
            var data2 = await _dataAccess.PlaneRepository.GetById(plane2.Id);
            Assert.True(data2.Status == PlaneStatus.Finished);
            var data3 = await _dataAccess.PlaneRepository.GetById(plane3.Id);
            Assert.True(data3.Status == PlaneStatus.Finished);
            var data4 = await _dataAccess.PlaneRepository.GetById(plane4.Id);
            Assert.True(data4.Status == PlaneStatus.Finished);
            airport.UpdateAirport();
        }
    }
}
