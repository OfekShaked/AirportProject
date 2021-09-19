using AirportProject.Commom.Enums;
using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Repositories
{
    class PlaneChangesRepository : BaseRepository<PlaneChangesDTO>, IPlaneChangesRepository
    {
        public PlaneChangesRepository(IMongoContext context) : base(context)
        {

        }

        public Task AddChange(string planeId, PlaneStationStatus status)
        {
            return Add(new PlaneChangesDTO { PlaneId = planeId, PlaneStationStatus = status });
        }
    }
}
