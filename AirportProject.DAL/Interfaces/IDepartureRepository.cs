using AirportProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Interfaces
{
    public interface IDepartureRepository : IRepository<DepartureDTO>
    {
        Task<DepartureDTO> AddDeparture(string planeId);
        Task SetDepartureFinished(string planeId);
        Task SetDepartureStarted(string planeId);
        Task<List<DepartureDTO>> GetAllFutureDepartures();
        Task<List<DepartureDTO>> GetAllWaitingDepartures();
        Task RemoveAllWaitingDepartures();
    }
}
