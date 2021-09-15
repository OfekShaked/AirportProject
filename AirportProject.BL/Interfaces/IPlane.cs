using AirportProject.BL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces
{
    public interface IPlane:IPlaneDTO
    {
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
