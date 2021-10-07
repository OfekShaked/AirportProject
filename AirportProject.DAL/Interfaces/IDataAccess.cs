using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Interfaces
{
    public interface IDataAccess
    {
         IAirportRepository AirportRepository { get; set; }
         IPlaneChangesRepository PlaneChangesRepository { get; set; }
         IPlaneRepository PlaneRepository { get; set; }
         IStationRepository StationRepository { get; set; }
         IArrivalRepository ArrivalRepository { get; set; }
         IDepartureRepository DepartureRepository { get; set; }
         IDbQueue TaskQueue { get; set; }

        string GetNewObjectId();
        
    }
}
