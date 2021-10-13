using AirportProject.BL.Models;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Models;
using AutoMapper;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL
{

    public class DTOMapper
    {
        public DTOMapper()
        {
        }

        public Airport AirportDTOToAirport(AirportDTO airportDTO,IDataAccess dataAccess=null,IUnitOfWork uow=null)
        {
            if (airportDTO == null) return null;
            Airport airport = null;
            if (dataAccess == null || uow == null)
            {
                 airport = new Airport()
                {
                    Id = airportDTO._id.ToString(),
                    DepartureEndingStations = airportDTO.DepartureEndingStations,
                    ArrivalEndingStations = airportDTO.ArrivalEndingStations,
                    DepartureStartingStations = airportDTO.DepartureStartingStations,
                    ArrivalStartingStations = airportDTO.ArrivalStartingStations,
                    Stations = StationsDTOToStations(airportDTO.DTOStations),
                };
            }
            else
            {
                 airport = new Airport(dataAccess, uow)
                {
                    Id = airportDTO._id.ToString(),
                    DepartureEndingStations = airportDTO.DepartureEndingStations,
                    ArrivalEndingStations = airportDTO.ArrivalEndingStations,
                    DepartureStartingStations = airportDTO.DepartureStartingStations,
                    ArrivalStartingStations = airportDTO.ArrivalStartingStations,
                    Stations = StationsDTOToStations(airportDTO.DTOStations)
                 };
            }
            return airport;
        }
        public AirportDTO AirportToAirportDTO(Airport airport)
        {
            if (airport == null) return null;
            var airportDTO = new AirportDTO {
                _id = new ObjectId(airport.Id),
                ArrivalEndingStations = airport.ArrivalEndingStations,
                ArrivalStartingStations = airport.ArrivalStartingStations,
                DepartureEndingStations = airport.DepartureEndingStations,
                DepartureStartingStations = airport.DepartureStartingStations,
                DTOStations = StationsToStationsDTO(airport.Stations),
            };
            return airportDTO;
        }
        public IPlane PlaneDTOToPlane(PlaneDTO planeDto)
        {
            if (planeDto == null) return null;
            IPlane plane = new Plane
            {
                Id = planeDto._id.ToString(),
                CurrentStationId = planeDto.CurrentStationId,
                Name = planeDto.Name,
                Status = (PlaneStatus)Enum.Parse(typeof(PlaneStatus), planeDto.Status)
            };
            return plane;
        }
        public PlaneDTO PlaneToPlaneDTO(IPlane plane)
        {
            if (plane == null) return null;
            var planeDto = new PlaneDTO
            {
                CurrentStationId = plane.CurrentStationId,
                Name = plane.Name,
                Status = plane.Status.ToString(),
                _id=new ObjectId(plane.Id),
            };
            return planeDto;
        }
        public Station StationDTOToStation(StationDTO stationDto)
        {
            if (stationDto == null) return null;
            var station = new Station()
            {
                ConnectedArrivalStations = stationDto.ConnectedArrivalStations,
                ConnectedDepartureStations = stationDto.ConnectedDepartureStations,
                CurrentPlaneInside = PlaneDTOToPlane(stationDto.CurrentPlaneInside),
                HandlingTime = stationDto.HandlingTime,
                Id = stationDto.Id,
                Name = stationDto.Name,
                _id = stationDto._id.ToString(),
            };
            return station;
        }
        public StationDTO StationToStationDTO(IStation station)
        {
            if (station == null) return null;
            var stationDTO = new StationDTO
            {
                _id = new ObjectId(station._id),
                ConnectedArrivalStations = station.ConnectedArrivalStations,
                ConnectedDepartureStations = station.ConnectedDepartureStations,
                CurrentPlaneInside = PlaneToPlaneDTO(station.CurrentPlaneInside),
                HandlingTime = station.HandlingTime,
                Id = station.Id,
                Name = station.Name,
                CurrentPlaneIdInside = station.CurrentPlaneInside == null ? null : station.CurrentPlaneInside.Id,
            };
            return stationDTO;
        }
        public Station StationAddInterfaces(IStation station,IDataAccess dataAccess,IUnitOfWork uow,IAirport airport)
        {
            if (station == null) return null;
            var newStation = new Station(station.Id, airport, dataAccess, uow)
            {
                _id = station._id,
                ConnectedArrivalStations = station.ConnectedArrivalStations,
                ConnectedDepartureStations = station.ConnectedDepartureStations,
                CurrentPlaneInside = station.CurrentPlaneInside,
                TrafficConnectedPaths = station.TrafficConnectedPaths,
                HandlingTime = station.HandlingTime,
                Name = station.Name,
                PlanesWaitingForStation = station.PlanesWaitingForStation
            };
            return newStation;
        }
        public List<StationDTO> StationsToStationsDTO(List<IStation> stations)
        {
            if (stations == null) return null;
            List<StationDTO> stationsDTO = new List<StationDTO>();
            foreach (var item in stations)
            {
                stationsDTO.Add(StationToStationDTO(item));
            }
            return stationsDTO;
        }
        public List<IStation> StationsDTOToStations(List<StationDTO> stationsDto)
        {
            if (stationsDto == null) return null;
            List<IStation> stations = new List<IStation>();
            foreach (var item in stationsDto)
            {
                stations.Add(StationDTOToStation(item));
            }
            return stations;
        }
    }
}
