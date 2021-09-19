using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Common.Interfaces
{
    public interface IArrivalMoveStation
    {
        /// <summary>
        /// Searches for the next free station to move to clear station as fast as possible while keeping it short.
        /// </summary>
        /// <param name="currentStation"></param>
        /// <param name="plane"></param>
        void MoveToNextStation(IPlane plane, IStation currentStation = null);
    }
}
