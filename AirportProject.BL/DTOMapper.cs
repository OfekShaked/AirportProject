using AirportProject.BL.Models;
using AirportProject.Common.Interfaces;
using AirportProject.Models.DAL;
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
        IMapper mapper;
        public DTOMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ObjectId, string>().ConvertUsing(o => o.ToString());
                cfg.CreateMap<string, ObjectId>().ConvertUsing(o => ObjectId.Parse(o));
                cfg.CreateMap<IPlane,PlaneDTO>().ForMember(x => x._id, opt => opt.MapFrom(src => src.Id)).ForMember(x => x.CreatedAt, opt => opt.Ignore()).ReverseMap().ForMember(x => x.Id, opt => opt.MapFrom(src => src._id));
                cfg.CreateMap<IStation,StationDTO>().ForMember(x => x._id, opt => opt.MapFrom(src => src._id)).ForMember(dest => dest.CreatedAt, opt => opt.Ignore()).ReverseMap().ForMember(x => x._id, opt => opt.MapFrom(src => src._id)).ForMember(x => x.PlanesWaitingForStation, opt => opt.Ignore()); 
                cfg.CreateMap<IAirport,AirportDTO>().ForMember(x => x._id, opt => opt.MapFrom(src=>src.Id)).ForMember(x => x.DTOStations, opt => opt.MapFrom(src => src.Stations)).ReverseMap().ForMember(x => x.Stations, opt => opt.MapFrom(src => src.DTOStations)).ForMember(x=>x.Id, opt=>opt.MapFrom(src=>src._id));
                cfg.CreateMap<Plane, PlaneDTO>().ForMember(x => x._id, opt => opt.MapFrom(src => src.Id)).ForMember(x => x.CreatedAt, opt => opt.Ignore()).ReverseMap().ForMember(x => x.Id, opt => opt.MapFrom(src => src._id));
                cfg.CreateMap<Station, StationDTO>().ForMember(x => x._id, opt => opt.MapFrom(src => src._id)).ForMember(dest=>dest.CreatedAt,  opt=>opt.Ignore()).ReverseMap().ForMember(x => x.PlanesWaitingForStation, opt => opt.Ignore()).ForMember(x => x._id, opt => opt.MapFrom(src => src._id));
                cfg.CreateMap<Airport, AirportDTO>().ForMember(x => x.DTOStations, opt => opt.MapFrom(src => src.Stations)).ForMember(x => x._id, opt => opt.MapFrom(src => src.Id));
                cfg.CreateMap<AirportDTO, Airport>().ForMember(x => x.Stations, opt => opt.MapFrom(src => src.DTOStations)).ForMember(x=>x.Arrivals, opt=>opt.Ignore()).ForMember(x=>x.Departures, opt=>opt.Ignore()).ForMember(x => x.Id, opt => opt.MapFrom(src => src._id));
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            mapper = configuration.CreateMapper();
        }

        public Airport AirportDTOToAirport(AirportDTO airportDTO)
        {
            Airport airport = new Airport
            {
                DepartureEndingStations = airportDTO.DepartureEndingStations,
                ArrivalEndingStations = airportDTO.ArrivalEndingStations,
                DepartureStartingStations = airportDTO.DepartureStartingStations,
                ArrivalStartingStations = airportDTO.ArrivalStartingStations,
                Stations = new List<IStation>(mapper.Map<List<Station>>(airportDTO.DTOStations))
            };
            return airport;
        }
        public AirportDTO AirportToAirportDTO(Airport airport)
        {
            return mapper.Map<AirportDTO>(airport);
        }
        public Plane PlaneDTOToPlane(PlaneDTO plane)
        {
            return mapper.Map<Plane>(plane);
        }
        public PlaneDTO PlaneToPlaneDTO(Plane plane)
        {
            return mapper.Map<PlaneDTO>(plane);
        }
        public Station StationDTOToStation(StationDTO station)
        {
            return mapper.Map<Station>(station);
        }
        public StationDTO StationToStationDTO(Station station)
        {
            return mapper.Map<StationDTO>(station);
        }
    }
}
