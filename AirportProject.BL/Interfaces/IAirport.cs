using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportProject.BL.DataStructures;

namespace AirportProject.BL.Interfaces
{
    public interface IAirport : IAirportDTO
    {
        Graph Arrivals
        {
            get;
            set;
        }
        Graph Departures
        {
            get;
            set;
        } 

        /// <summary>
        /// Listen to the simulator to get new arrivals or departures
        /// </summary>
        void GetPlaneFromSimulator(IPlane plane);
        /// <summary>
        /// Create new airport when server loads up based on the user preference or if not recieved
        /// anyting based on the default configuration
        /// </summary>
        /// <param name="airportDTO"></param>
        void CreateNewAirport(IAirportDTO airportDTO=null);
        /// <summary>
        /// Search for an open spot for a plane to land
        /// </summary>
        /// <param name="planeId"></param>
        void SearchForFreeLanding(IPlane plane);
        /// <summary>
        /// Search for an open spot for the plane to depart
        /// </summary>
        /// <param name="planeId"></param>
        void SearchForFreeDeparting(IPlane plane);
        /// <summary>
        /// Sends a signal for a specific plane in a queue for a station to move to a new station
        /// </summary>
        /// <param name="planeId"></param>
        void SignalPlaneToMove(IPlane plane);
        /// <summary>
        /// Gets a signal from a station that it finished handling a plane
        /// </summary>
        void StationFinishedHandlingPlaneCallBack(IStation station);
        /// <summary>
        /// Signals that the plane finished and departed successfully
        /// </summary>
        void SignalPlaneFinishedDeparture(IPlane plane);
        /// <summary>
        /// Signals that the plane finished all landing and is in the airport.
        /// </summary>
        /// <param name="plane"></param>
        void SignalPlaneFinishedArrival(IPlane plane);
        /// <summary>
        /// Removes a plane from all the queues he was from the previous station
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="previousStation"></param>
        void RemovePlaneFromAllPreviousStationQueues(IPlane plane, IStation previousStation);
    }
}
