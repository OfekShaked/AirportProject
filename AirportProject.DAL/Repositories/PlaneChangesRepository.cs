using AirportProject.Commom.Enums;
using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Models;
using MongoDB.Bson;
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

        public async Task AddChange(string planeId,string stationId, PlaneStationStatus status)
        {
            var change = new PlaneChangesDTO {PlaneId = planeId, StationId = stationId, Status = status };
            await DbSet.InsertOneAsync(change);
        }
    }
}
