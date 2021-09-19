using AirportProject.Commom.Enums;
using AirportProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Interfaces
{
    public interface IPlaneChangesRepository : IRepository<PlaneChangesDTO>
    {
        Task AddChange(string planeId, PlaneStationStatus status);
    }
}
