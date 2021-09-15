using AirportProject.BL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces
{
    public interface IDeparture : IDepartureDTO
    {
        /// <summary>
        /// Add a new plane to the queue of departures
        /// </summary>
        /// <param name="plane"></param>
        void AddNewPlaneToQueue(IPlane plane);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plane"></param>
        void RemovePlaneFromQueue(IPlane plane);
    }
}
