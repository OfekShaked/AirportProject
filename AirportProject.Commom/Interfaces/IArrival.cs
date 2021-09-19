using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Common.Interfaces
{
    public interface IArrival 
    {
        List<IPlane> Landings
        {
            get;
            set;
        }
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
