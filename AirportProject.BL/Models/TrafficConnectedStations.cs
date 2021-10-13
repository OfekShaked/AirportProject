using AirportProject.Commom.Interfaces;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class TrafficConnectedStations : ITrafficConnectedStations
    {
        private IAirport _airport;
        private object _lockObject = new object();
        public LinkedList<int> StationsInPath { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public Queue<ISpecialTrafficQueueModel> PlanesWaiting { get; set; }
        public IPlane CurrentPlaneInside { get; set; }
        public int FirstArrivalStation { get; set; }
        public int LastArrivalStation { get; set; }
        public int FirstDepartureStation { get; set; }
        public int LastDepartureStation { get; set; }

        public TrafficConnectedStations(IAirport airport)
        {
            _airport = airport;
            StationsInPath = new LinkedList<int>();
            PlanesWaiting = new Queue<ISpecialTrafficQueueModel>();
            IsBlocked = false;
        }
        public void SetName(List<int> stations)
        {
            Name = GetNameFromList(stations);
        }
        public void EnterStation(IStation stationToEnter, IPlane plane)
        {
            lock (_lockObject)
            {
                var nodeFound = StationsInPath.Find(stationToEnter.Id);
                if (nodeFound == null) throw new Exception("Station not in the special path");
                if (plane.Status == PlaneStatus.Arrival)
                {
                    if (FirstArrivalStation == stationToEnter.Id)
                    {
                        IsBlocked = true;
                    }
                }
                else
                {
                    if (FirstDepartureStation == stationToEnter.Id)
                    {
                        IsBlocked = true;
                    }
                }
                CurrentPlaneInside = plane;
            }
        }

        public void ExitStation(IStation stationToExit, IPlane plane)
        {
            var nodeFound = StationsInPath.Find(stationToExit.Id);
            if (nodeFound == null) throw new Exception("Station not in the special path");
            bool isExit = false;
            foreach (var stationId in StationsInPath)
            {
                if (_airport.Stations[stationId].IsPlaneInside())
                {
                    isExit = true;
                    break;
                }
            }
            if (isExit)
            {
                ClearTrafficStations();
            }
            
        }
        public bool CompareNameToList(List<int> stations)
        {
            return this.Name == GetNameFromList(stations);
        }
        private void ClearTrafficStations()
        {
            IsBlocked = false;
            CurrentPlaneInside = null;
        }
        private string GetNameFromList(List<int> stations)
        {
            stations.Sort();
            StringBuilder nameStr = new StringBuilder();
            foreach (var stationId in stations)
            {
                nameStr.Append(stationId.ToString());
            }
            return nameStr.ToString(); ;
        }

        public void AddToQueue(IPlane plane)
        {
            //if plane's current station is not in the path it means that it tried to enter the special station
            if (!StationsInPath.Contains(plane.CurrentStationId))
            {
                PlanesWaiting.Enqueue(new SpecialTrafficQueueModel(plane));
            }  
        }
        public void RemoveFromQueue(IPlane plane)
        {
            if (PlanesWaiting.Count != 0)
            {
                PlanesWaiting = new Queue<ISpecialTrafficQueueModel>(PlanesWaiting.Where(p => p.Plane != plane));
            }
        }
    }
}
