using AirportProject.Commom.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Common.Interfaces
{
    public interface IStation
    {
        string _id { get; set; }
        int Id
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        IPlane CurrentPlaneInside 
        { 
            get; 
            set; 
        }
        TimeSpan HandlingTime
        {
            get;
            set;
        }
        List<int> ConnectedDepartureStations
        {
            get;
            set;
        }
        List<int> ConnectedArrivalStations
        {
            get;
            set;
        }
        /// <summary>
        /// Queue of planes waiting to enter the station
        /// </summary>
        Queue<IPlane> PlanesWaitingForStation
        {
            get;
            set;
        }
        List<ITrafficConnectedStations> TrafficConnectedPaths { get; set; }
        bool IsPlaneInside(IPlane plane = null);
        void SetCurrentPlaneInside(IPlane plane);
        /// <summary>
        /// Sends a signals that contains the current station id to the airport that the current station finished handling the plane
        /// </summary>
        void SignalStationFinishedHandlingPlane();
        /// <summary>
        /// Removes a plane if exist from the station
        /// </summary>
        void SetStationFree();
        /// <summary>
        /// Adds a plane to the current station
        /// </summary>
        bool AddPlaneToStation(IPlane plane);
        /// <summary>
        /// Adds a plane for the station queue
        /// </summary>
        /// <param name="plane"></param>
        void AddPlaneToQueue(IPlane plane);
        /// <summary>
        /// Removes a plane from the queue
        /// </summary>
        /// <param name="plane"></param>
        void RemovePlaneFromQueue(IPlane plane);
        /// <summary>
        /// Restart the station plane handling if plane is inside on new load
        /// </summary>
        void RestartStationHandling();
    }
}
