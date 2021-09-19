using AirportProject.Common.DataStructures;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using AirportProject.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Airport : IAirport
    {
        public string Id { get; set; }
        public Graph Arrivals { get; set; }
        public Graph Departures { get; set; }
        public List<IStation> Stations { get; set; }
        public List<int> ArrivalStartingStations { get; set; }
        public List<int> DepartureStartingStations { get; set; }
        public List<int> DepartureEndingStations { get; set; }
        public List<int> ArrivalEndingStations { get; set; }
        private DepartureMoveStation _departureMoveStation;
        private ArrivalMoveStation _arrivalMoveStation;
        private DataAccess _dataAccess;
        private IUnitOfWork _uow;
        private DTOMapper _mapper;

        public Airport()
        {

        }
        public Airport(IMongoContext context, IUnitOfWork uow,IAirport airport = null)
        {
            _dataAccess = new DataAccess(context);
            _uow = uow;
            Id = airport == null ? _dataAccess.GetNewObjectId() : airport.Id; 
            _departureMoveStation = new DepartureMoveStation(this, _dataAccess, _uow);
            _arrivalMoveStation = new ArrivalMoveStation(this,_dataAccess,_uow);
            _mapper = new DTOMapper();
            CreateNewAirport(airport);
        }
        public void CreateNewAirport(IAirport airportDTO = null)
        {
            if (airportDTO == null)
            {
                CreateDefaultStations();
            }
            StationsToGraphsConvertor();
        }


        public void StationFinishedHandlingPlaneCallBack(IStation station)
        {
            if (station.CurrentPlaneInside == null)
            {
                throw new Exception("Station is empty");
            }
            if (station.CurrentPlaneInside.Status == PlaneStatus.Departure)
            {
                _departureMoveStation.MoveToNextStation(station.CurrentPlaneInside, station);
            }
            else if (station.CurrentPlaneInside.Status == PlaneStatus.Arrival)
            {
                _arrivalMoveStation.MoveToNextStation(station.CurrentPlaneInside, station);
            }
            else
            {
                throw new Exception("Plane is not in a correct status");
            }
        }

        public void GetPlaneFromSimulator(IPlane plane)
        {
            if (plane.Status == PlaneStatus.Arrival)
            {
                Task.Run(async () =>
                {
                    await _dataAccess.PlaneRepository.Add(_mapper.PlaneToPlaneDTO((Plane)plane));
                    await _dataAccess.ArrivalRepository.AddArrival(plane.Id);
                    _uow.Commit();
                });
                
                SearchForFreeLanding(plane);
            }
            else
            {
                Task.Run(async () =>
                {
                    await _dataAccess.PlaneRepository.Add(_mapper.PlaneToPlaneDTO((Plane)plane));
                    await _dataAccess.DepartureRepository.AddDeparture(plane.Id);
                    _uow.Commit();
                });
                SearchForFreeDeparting(plane);
            }
            
        }

        public void SearchForFreeDeparting(IPlane plane)
        {
            _departureMoveStation.MoveToNextStation(plane);
        }

        public void SearchForFreeLanding(IPlane plane)
        {
            _arrivalMoveStation.MoveToNextStation(plane);
        }

        public void SignalPlaneToMove(IPlane plane)
        {
            if (plane.Status == PlaneStatus.Departure)
            {
                _departureMoveStation.MoveToNextStation(plane, plane.CurrentStation);
            }
            else if (plane.Status == PlaneStatus.Arrival)
            {
                _arrivalMoveStation.MoveToNextStation(plane, plane.CurrentStation);
            }
            else
            {
                throw new Exception("Plane is not in a correct status");
            }
        }
        public void SignalPlaneFinishedDeparture(IPlane plane)
        {
            plane.Status = PlaneStatus.Finished;
            
        }

        public void SignalPlaneFinishedArrival(IPlane plane)
        {
            plane.Status = PlaneStatus.Finished;
        }
        private void CreateDefaultStations()
        {
            Stations = new List<IStation>
                {
                    new Station(0,this,_dataAccess,_uow){Name="Station0", ConnectedDepartureStations=new List<int>(), ConnectedArrivalStations = new List<int>{1} },
                    new Station(1,this,_dataAccess,_uow){Name="Station1", ConnectedDepartureStations=new List<int>(), ConnectedArrivalStations = new List<int>{2} },
                    new Station(2,this,_dataAccess,_uow){Name="Station2", ConnectedDepartureStations=new List<int>(), ConnectedArrivalStations = new List<int>{3} },
                    new Station(3,this,_dataAccess,_uow){Name="Station3", ConnectedDepartureStations=new List<int>(), ConnectedArrivalStations = new List<int>{4} },
                    new Station(4,this,_dataAccess,_uow){Name="Station4", ConnectedDepartureStations=new List<int>(), ConnectedArrivalStations = new List<int>{5,6} },
                    new Station(5,this,_dataAccess,_uow){Name="Station5", ConnectedDepartureStations=new List<int>{7}, ConnectedArrivalStations = new List<int>() },
                    new Station(6,this,_dataAccess,_uow){Name="Station6", ConnectedDepartureStations=new List<int>{7}, ConnectedArrivalStations = new List<int>() },
                    new Station(7,this,_dataAccess,_uow){Name="Station7", ConnectedDepartureStations=new List<int>{3}, ConnectedArrivalStations = new List<int>() },
                };
            ArrivalStartingStations = new List<int>
            {
                0
            };
            DepartureStartingStations = new List<int>
            {
                5,6
            };
            ArrivalEndingStations = new List<int>
            {
                5,6
            };
            DepartureEndingStations = new List<int>
            {
                3
            };
        }
        private void StationsToGraphsConvertor()
        {
            Arrivals = new Graph(Stations.Count);
            Departures = new Graph(Stations.Count);
            for (int i = 0; i < Stations.Count; i++)
            {
                var arrivals = Stations[i].ConnectedArrivalStations;
                for (int j = 0; j < arrivals.Count; j++)
                {
                    Arrivals.AddEdge(i, arrivals[j],(int)Stations[i].HandlingTime.TotalMilliseconds);
                }
                var departures = Stations[i].ConnectedDepartureStations;
                for (int j = 0; j < departures.Count; j++)
                {
                    Departures.AddEdge(i, departures[j], (int)Stations[i].HandlingTime.TotalMilliseconds);
                }
            }
        }

        public void RemovePlaneFromAllPreviousStationQueues(IPlane plane, IStation previousStation)
        {
            List<int> connectedStationsIds;
            if(plane.Status== PlaneStatus.Departure)
            {
                if (previousStation != null)
                {
                    connectedStationsIds = previousStation.ConnectedDepartureStations;
                }
                else
                {
                    connectedStationsIds = DepartureStartingStations;
                }
            }
            else
            {
                if (previousStation != null)
                {
                    connectedStationsIds = previousStation.ConnectedArrivalStations;
                }
                else
                {
                    connectedStationsIds = ArrivalStartingStations;
                }
            }
            foreach (var stationId in connectedStationsIds)
            {
                Stations[stationId].RemovePlaneFromQueue(plane);
            }
            
        }
    }
}
