using AirportProject.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Common.Interfaces
{
    public interface IPlane
    {
        string Id
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        PlaneStatus Status
        {
            get;
            set;
        }
        /// <summary>
        /// Current station of the plane
        /// -1 indicates Waiting for landing
        /// -2 indicates Waiting for departure
        /// -3 indicates finished handling plane in airport
        /// </summary>
        int CurrentStationId
        {
            get;
            set;
        }
        IStation CurrentStation
        {
            get;
            set;
        }
        /// <summary>
        /// Sets the current station of the plane and the current station id
        /// </summary>
        /// <param name="station"></param>
        void SetCurrentStation(IStation station);
    }
}
