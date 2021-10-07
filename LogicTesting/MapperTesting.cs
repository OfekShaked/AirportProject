using AirportProject.BL;
using AirportProject.BL.Models;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using AirportProject.Models.DAL;
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
    public class MapperTesting
    {
        DTOMapper mapper;
        IMongoContext _context;
        IUnitOfWork _uow;
        DataAccess _dataAccess;
        public MapperTesting()
        {
            var appSettings = @"{""MongoSettings"":{
            ""Connection"" : ""mongodb://localhost:27017"",
            ""DatabaseName"" : ""AirportProjectTesting"",
            }}";
            mapper = new DTOMapper();
            Microsoft.Extensions.Configuration.IConfigurationBuilder builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.AddJsonStream(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
            var configuration = builder.Build();
            _context = new MongoContext(configuration);
            _uow = new UnitOfWork(_context);
            _dataAccess = new DataAccess(_context);
        }
        [Fact]
        public void TestPlaneToPlaneDTO()
        {
            IPlane plane = new Plane { Id = ObjectId.GenerateNewId().ToString(), Name = "planetoAus", Status = PlaneStatus.Departure };
            var planeDTO = mapper.PlaneToPlaneDTO((Plane)plane);
            Assert.True(planeDTO.GetType() == typeof(PlaneDTO));
            Assert.True(plane.Id == planeDTO._id.ToString());
            Assert.True(plane.Name == planeDTO.Name);
            Assert.True(plane.Status == planeDTO.Status);
        }
        [Fact]
        public void TestPlaneDTOToPlane()
        {
            PlaneDTO planeDTO = new PlaneDTO { _id = ObjectId.GenerateNewId(), Name = "planeToAus", Status = PlaneStatus.Arrival };
            var plane = mapper.PlaneDTOToPlane(planeDTO);
            Assert.True(plane.GetType() == typeof(Plane));
            Assert.True(plane.Id == planeDTO._id.ToString());
            Assert.True(plane.Name == planeDTO.Name);
            Assert.True(plane.Status == planeDTO.Status);
        }
        [Fact]
        public void TestStationToStationDTO()
        {
            IStation station = new Station(1,new Airport(_dataAccess,_uow),_dataAccess,_uow) {_id=ObjectId.GenerateNewId().ToString(), Name = "Station1", ConnectedArrivalStations = new List<int> { 1, 2 }, ConnectedDepartureStations = new List<int> { 3, 4 } };
            var stationDTO = mapper.StationToStationDTO((Station)station);
            Assert.True(stationDTO.GetType() == typeof(StationDTO));
            Assert.True(station.Id == stationDTO.Id);
            Assert.True(station.Name == stationDTO.Name);
            Assert.True(station.ConnectedArrivalStations.Count == stationDTO.ConnectedArrivalStations.Count);
        }
        [Fact]
        public void TestStationDTOToStation()
        {
            StationDTO stationDTO = new StationDTO { Name = "Station1", ConnectedArrivalStations = new List<int> { 1, 2 }, ConnectedDepartureStations = new List<int> { 3, 4 } };
            var station = mapper.StationDTOToStation(stationDTO);
            Assert.True(station.GetType() == typeof(Station));
            Assert.True(station.Id == stationDTO.Id);
            Assert.True(station.Name == stationDTO.Name);
            Assert.True(station.ConnectedArrivalStations.Count == stationDTO.ConnectedArrivalStations.Count);
        }
        [Fact]
        public void TestAirportToAirportDTO()
        {
            IAirport airport = new Airport (_dataAccess, _uow) {Id = ObjectId.GenerateNewId().ToString(), Stations = new List<IStation> { new Station(1, new Airport(), _dataAccess, _uow) {_id=ObjectId.GenerateNewId().ToString(), Name = "Station1", ConnectedArrivalStations = new List<int> { 1, 2 }, ConnectedDepartureStations = new List<int> { 3, 4 } } } };
            var airportDTO = mapper.AirportToAirportDTO((Airport)airport);
            Assert.True(airportDTO.GetType() == typeof(AirportDTO));
            Assert.True(airportDTO.DTOStations.Count == airport.Stations.Count);
        }
        [Fact]
        public void TestAirportDTOToAirport()
        {
            AirportDTO airportDTO = new AirportDTO { DTOStations = new List<StationDTO> { new StationDTO { Name = "Station1", ConnectedArrivalStations = new List<int> { 1, 2 }, ConnectedDepartureStations = new List<int> { 3, 4 } } } };
            var airport = mapper.AirportDTOToAirport(airportDTO);
            Assert.True(airport.GetType() == typeof(Airport));
            Assert.True(airportDTO.DTOStations.Count == airport.Stations.Count);
        }
    }
}
