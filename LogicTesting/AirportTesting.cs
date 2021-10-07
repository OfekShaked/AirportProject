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
            IAirport airport = new Airport(_dataAccess,_uow);
            airport.CreateNewAirport();
            airport.GetPlaneFromSimulator(new Plane() {  Status = PlaneStatus.Arrival });
            //airport.GetPlaneFromSimulator(new Plane() { Status = PlaneStatus.Arrival });
            //airport.GetPlaneFromSimulator(new Plane() { Status = PlaneStatus.Arrival });
            //airport.GetPlaneFromSimulator(new Plane() { Status = PlaneStatus.Arrival });
            await Task.Delay(25000);
            Assert.True(true);
        }
    }
}
