using AirportProject.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces.DTO
{
    public interface IPlaneDTO
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
    }
}
