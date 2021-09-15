using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces
{
    public interface IStation: IStationDTO
    {
        /// <summary>
        /// Queue of planes waiting to enter the station
        /// </summary>
        Queue<IPlane> PlanesWaitingForStation
        {
            get;
            set;
        }
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
        void AddPlaneToStation(IPlane plane);
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
    }
}
