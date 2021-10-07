using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Repositories
{
    public class DepartureRepository : BaseRepository<DepartureDTO>,IDepartureRepository
    {
        public DepartureRepository(IMongoContext context):base(context)
        {

        }
        public async Task AddDeparture(string planeId)
        {
            await Add(new DepartureDTO {_id=ObjectId.GenerateNewId(), PlaneId = planeId, IsFinished = false });
        }

        public async Task SetDepartureFinished(string planeId)
        {
                var filter = Builders<DepartureDTO>.Filter.Eq("PlaneId", planeId);
                var update = Builders<DepartureDTO>.Update
                    .Set(p => p.IsFinished, true);
            var result = await DbSet.UpdateOneAsync(filter, update);
        }
    }
}
