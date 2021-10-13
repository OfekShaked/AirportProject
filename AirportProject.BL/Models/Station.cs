using AirportProject.BL.Exceptions;
using AirportProject.Commom.Enums;
using AirportProject.Commom.Interfaces;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Models;
using MongoDB.Bson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Station : IStation
    {
        public string _id { get; set; }
        public int Id { get; set; }
        private object _lockObject = new object();
        private object _lockFreeObject = new object();
        public string Name { get; set; }
        public IPlane CurrentPlaneInside { get; set; }
        public TimeSpan HandlingTime { get; set; }
        public List<int> ConnectedDepartureStations { get; set; }
        public List<int> ConnectedArrivalStations { get; set; }
        public Queue<IPlane> PlanesWaitingForStation { get; set; }
        public List<ITrafficConnectedStations> TrafficConnectedPaths { get; set; }
        private IAirport _airport;
        private IDataAccess _dataAccess;
        private IUnitOfWork _uow;
        private DTOMapper _mapper;
        private Timer _safetyTimer;

        public Station()
        {
            PlanesWaitingForStation = new Queue<IPlane>();
            TrafficConnectedPaths = new List<ITrafficConnectedStations>();
        }
        public Station(int id, IAirport airport, IDataAccess dataAccess, IUnitOfWork uow, TimeSpan? handlingTime = null)
        {
            _mapper = new DTOMapper();
            _dataAccess = dataAccess;
            _uow = uow;
            PlanesWaitingForStation = new Queue<IPlane>();
            TrafficConnectedPaths = new List<ITrafficConnectedStations>();
            if (handlingTime == null) HandlingTime = new TimeSpan(0, 0, 5);
            else HandlingTime = (TimeSpan)handlingTime;
            Id = id;
            _id = ObjectId.GenerateNewId().ToString();
            _airport = airport;
            SetUpTimer();
        }
        public bool AddPlaneToStation(IPlane plane)
        {
            var currentId = plane.Id;
            lock (_lockObject)
            {
                try
                {
                    if (CurrentPlaneInside != null)
                    {
                        return false;
                    }
                    RestartTimer();
                    CurrentPlaneInside = plane;
                    _airport.RemovePlaneFromAllPreviousStationQueues(plane, plane.CurrentStation);
                    plane.CurrentStation = this;
                    JoinSpecialTrafficPaths(plane);
                    StartStationHandling();
                    _airport.UpdateAirport();
                    _dataAccess.TaskQueue.AddTask(Task.Run(async () =>
                    {
                        await _dataAccess.PlaneChangesRepository.AddChange(currentId, Id.ToString(), PlaneStationStatus.EnterStation);
                        await _uow.Commit();
                    }));
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        private async void StartStationHandling()
        {
            await Task.Delay(HandlingTime);
            SignalStationFinishedHandlingPlane();
        }

        public void SetStationFree()
        {
            RestartTimer();
            if (CurrentPlaneInside == null) return;
            var currentId = CurrentPlaneInside.Id;
            _dataAccess.TaskQueue.AddTask(Task.Run(async () =>
            {
                await _dataAccess.PlaneChangesRepository.AddChange(currentId, Id.ToString(), PlaneStationStatus.ExitStation);
                await _uow.Commit();
            }));
            LeaveSpecialTrafficPaths(CurrentPlaneInside);
            CurrentPlaneInside = null;
            _airport.UpdateAirport();
            if (PlanesWaitingForStation.Count != 0)
            {
                if (TrafficConnectedPaths.Count == 0)
                {
                    _airport.SignalPlaneToMove(PlanesWaitingForStation.Peek());
                }
                else
                {
                    SignalNextPlaneInSpecialQueuesToMove();
                }
            }

        }

        public void SignalStationFinishedHandlingPlane()
        {
            _airport.StationFinishedHandlingPlaneCallBack(this);
        }

        public void AddPlaneToQueue(IPlane plane)
        {
            if (PlanesWaitingForStation.Where(p=>p?.Id==plane.Id).Count()==0)
            {
                PlanesWaitingForStation.Enqueue(plane);
                AddPlaneToSpecialQueues(plane);
            }
        }

        public void RemovePlaneFromQueue(IPlane plane)
        {
            if (PlanesWaitingForStation.Count != 0)
            {
                PlanesWaitingForStation = new Queue<IPlane>(PlanesWaitingForStation.Where(p => p?.Id != plane.Id));
            }
            RemovePlaneFromSpecialQueues(plane);
        }

        public bool IsPlaneInside(IPlane plane = null)
        {
            lock (_lockFreeObject)
            {
                bool isInside = false;
                if (TrafficConnectedPaths.Count == 0)
                {
                    isInside = CurrentPlaneInside != null;
                }
                else
                {
                    isInside = IsTrafficConnectedStationsEmpty(plane);
                }
                return isInside;
            }
        }

        public void SetCurrentPlaneInside(IPlane plane)
        {
            CurrentPlaneInside = plane;
        }

        public void RestartStationHandling()
        {
            if (CurrentPlaneInside != null)
            {
                StartStationHandling();
            }
        }
        /// <summary>
        /// Joins special traffic paths if they exist. acts like a traffic light.
        /// </summary>
        /// <param name="plane"></param>
        private void JoinSpecialTrafficPaths(IPlane plane)
        {
            if (TrafficConnectedPaths.Count != 0)
            {
                foreach (var specialPath in TrafficConnectedPaths)
                {
                    specialPath.EnterStation(this, plane);
                }
            }
        }
        private void LeaveSpecialTrafficPaths(IPlane plane)
        {
            if (TrafficConnectedPaths.Count != 0)
            {
                foreach (var specialPath in TrafficConnectedPaths)
                {
                    specialPath.ExitStation(this, plane);
                }
            }
        }
        private void SignalNextPlaneInSpecialQueuesToMove()
        {
            List<ISpecialTrafficQueueModel> firstInQueueModels = new List<ISpecialTrafficQueueModel>();
            foreach (var specialPath in TrafficConnectedPaths)
            {
                if (specialPath.PlanesWaiting.Count != 0 && specialPath.IsBlocked == false)
                {
                    firstInQueueModels.Add(specialPath.PlanesWaiting.Peek());
                }
            }
            if (firstInQueueModels.Count != 0)
            {
                _airport.SignalPlaneToMove(firstInQueueModels.OrderBy(m => m.TimeAddedToQueue).First().Plane);
            }
        }
        private void RemovePlaneFromSpecialQueues(IPlane plane)
        {
            foreach (var specialStation in TrafficConnectedPaths)
            {
                specialStation.RemoveFromQueue(plane);
            }
        }
        private void AddPlaneToSpecialQueues(IPlane plane)
        {
            foreach (var specialStation in TrafficConnectedPaths)
            {
                specialStation.AddToQueue(plane);
            }
        }
        private bool IsTrafficConnectedStationsEmpty(IPlane plane)
        {
            foreach (var specialPath in TrafficConnectedPaths)
            {
                if (specialPath.CurrentPlaneInside == null || specialPath.CurrentPlaneInside == plane)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private void SetUpTimer()
        { 
            _safetyTimer = new Timer(StartCheckAgain, null, Convert.ToInt32(HandlingTime.TotalMilliseconds * 2), Convert.ToInt32(HandlingTime.TotalMilliseconds * 2));
        }
        private void RestartTimer()
        {
            _safetyTimer.Dispose();
            SetUpTimer();
        }
        private void StartCheckAgain(object obj)
        {
            if (CurrentPlaneInside != null)
            {
                _airport.StationFinishedHandlingPlaneCallBack(this);
            }
            else
            {
                if (PlanesWaitingForStation.Count != 0)
                {
                    _airport.SignalPlaneToMove(PlanesWaitingForStation.Peek());
                }
            }
        }
    }
}
