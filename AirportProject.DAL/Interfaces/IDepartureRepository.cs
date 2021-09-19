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
        Task AddDeparture(string planeId);
        Task SetDepartureFinished(string planeId);
    }
}
