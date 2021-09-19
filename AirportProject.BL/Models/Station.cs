using AirportProject.BL.Exceptions;
using AirportProject.Commom.Enums;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Station : IStation
    {
        public string _id { get; set; }
        public int Id { get; set; }
        private object _lockObject = new object();
        public string Name { get; set; }
        public IPlane CurrentPlaneInside { get; set; }
        public TimeSpan HandlingTime { get; set; }
        public List<int> ConnectedDepartureStations { get; set; }
        public List<int> ConnectedArrivalStations { get; set; }
        public Queue<IPlane> PlanesWaitingForStation { get; set; }
        private IAirport _airport;
        private DataAccess _dataAccess;
        private IUnitOfWork _uow;
        private DTOMapper _mapper;

        public Station()
        {
            PlanesWaitingForStation = new Queue<IPlane>();
        }
        public Station(int id,IAirport airport, DataAccess dataAccess,IUnitOfWork uow, TimeSpan? handlingTime=null)
        {
            _mapper = new DTOMapper();
            _dataAccess = dataAccess;
            _uow = uow;
            PlanesWaitingForStation = new Queue<IPlane>();
            if (handlingTime == null) HandlingTime = new TimeSpan(0, 0, 2);
            else HandlingTime = (TimeSpan)handlingTime;
            Id = id;
            _airport = airport;
        }
        public void AddPlaneToStation(IPlane plane)
        {
            lock (_lockObject)
            {
                if(CurrentPlaneInside != null)
                {
                    throw new StationException("Station is not empty");
                }
                if(PlanesWaitingForStation.Count!=0&&!PlanesWaitingForStation.Peek().Equals(plane))
                {
                    throw new StationException("Plane is not the first in queue");
                }
                CurrentPlaneInside = plane;
                _airport.RemovePlaneFromAllPreviousStationQueues(plane, plane.CurrentStation);
                plane.SetCurrentStation(this);
                Task.Run(async () =>
                {
                    await _dataAccess.AirportRepository.Update(_mapper.AirportToAirportDTO((Airport)_airport));
                    await _dataAccess.PlaneChangesRepository.AddChange(CurrentPlaneInside.Id, PlaneStationStatus.Enter);
                    _uow.Commit();
                });
                StartStationHandling();
            }
        }
        private void StartStationHandling()
        {
            Task.Run(async () =>
            {
                await Task.Delay(HandlingTime);
                SignalStationFinishedHandlingPlane();
            });
        }

        public void SetStationFree()
        {
            Task.Run(async () =>
            {
                await _dataAccess.AirportRepository.Update(_mapper.AirportToAirportDTO((Airport)_airport));
                await _dataAccess.PlaneChangesRepository.AddChange(CurrentPlaneInside.Id, PlaneStationStatus.Exit);
                _uow.Commit();
            });
            CurrentPlaneInside = null;
            if (PlanesWaitingForStation.Count != 0)
            {
                _airport.SignalPlaneToMove(PlanesWaitingForStation.Peek());
            }
            
        }

        public void SignalStationFinishedHandlingPlane()
        {
            _airport.StationFinishedHandlingPlaneCallBack(this);
        }

        public void AddPlaneToQueue(IPlane plane)
        {
            PlanesWaitingForStation.Enqueue(plane);
        }

        public void RemovePlaneFromQueue(IPlane plane)
        {
            if (PlanesWaitingForStation.Count != 0)
            {
                PlanesWaitingForStation = new Queue<IPlane>(PlanesWaitingForStation.Where(p => p != plane));
            }
        }
    }
}
