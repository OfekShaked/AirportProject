using AirportProject.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Interfaces
{
    public interface IPlaneRepository : IRepository<PlaneDTO>
    {
        Task SetPlaneStatusFinished(string planeId);
    }
}
