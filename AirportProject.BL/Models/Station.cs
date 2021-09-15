using AirportProject.BL.Exceptions;
using AirportProject.BL.Interfaces;
using AirportProject.BL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Station : IStation
    {
        public int Id { get; set; }
        private object _lockObject = new object();
        public string Name { get; set; }
        public IPlane CurrentPlaneInside { get; set; }
        public TimeSpan HandlingTime { get; set; }
        public List<int> ConnectedDepartureStations { get; set; }
        public List<int> ConnectedArrivalStations { get; set; }
        public Queue<IPlane> PlanesWaitingForStation { get; set; }
        private IAirport _airport;

        public Station(int id,IAirport airport, TimeSpan? handlingTime=null)
        {
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
                if (plane.CurrentStation != null)
                {
                    _airport.RemovePlaneFromAllPreviousStationQueues(plane, plane.CurrentStation);
                }
                plane.SetCurrentStation(this);
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
