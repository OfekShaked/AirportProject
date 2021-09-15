using AirportProject.BL.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces
{
    public interface IAirportDTO
    {
        List<IStation> Stations
        {
            get;
            set;
        }
        List<int> ArrivalStartingStations
        {
            get;
            set;
        }
        List<int> DepartureStartingStations
        {
            get;
            set;
        }
        List<int> DepartureEndingStations
        {
            get;
            set;
        }
        List<int> ArrivalEndingStations
        {
            get;
            set;
        }
    }
}
