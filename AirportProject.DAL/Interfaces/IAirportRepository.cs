using AirportProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Interfaces
{
    public interface IAirportRepository : IRepository<AirportDTO>
    {
        Task AddOrUpdate(AirportDTO airport);
        Task<AirportDTO> GetAirport(string id = null);
        Task UpdateAirport(AirportDTO airport);

    }
}
