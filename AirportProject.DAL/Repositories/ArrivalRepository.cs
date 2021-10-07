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
    public class ArrivalRepository : BaseRepository<ArrivalDTO>, IArrivalRepository
    {
        public ArrivalRepository(IMongoContext context): base(context)
        {

        }

        public async Task AddArrival(string planeId)
        {
            await Add(new ArrivalDTO {_id=ObjectId.GenerateNewId(), PlaneId = planeId, IsFinished = false });
        }

        public async Task SetArrivalFinished(string planeId)
        {
            var filter = Builders<ArrivalDTO>.Filter.Eq("PlaneId", planeId);
            var update = Builders<ArrivalDTO>.Update
               .Set(p => p.IsFinished, true);
            var result = await DbSet.UpdateOneAsync(filter, update);
        }
    }
}
