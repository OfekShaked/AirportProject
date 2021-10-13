using AirportProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces
{
    public interface INotifyUpdates
    {
        Action<AirportDTO> UpdateAirport { get; set; }
        Action<ArrivalDTO> AddArrival { get; set; }
        Action<string> RemoveArrival { get; set; }
        Action<DepartureDTO> AddDeparture { get; set; }
        Action<string> RemoveDeparture { get; set; }
        Action<List<DepartureDTO>> UpdatePlannedDepartures { get; set; }
        Action<List<ArrivalDTO>> UpdatePlannedArrivals { get; set; }
    }
}
