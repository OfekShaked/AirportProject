using AirportProject.Commom.Enums;
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

        public async Task<ArrivalDTO> AddArrival(string planeId)
        {
           var newArrival = new ArrivalDTO { _id = ObjectId.GenerateNewId(), PlaneId = planeId, AirportPlaneStatus = AirportPlaneStatus.Waiting.ToString() };
            await DbSet.InsertOneAsync(newArrival);
            return newArrival;
        }

        public async Task<List<ArrivalDTO>> GetAllFutureArrivals()
        {
            var filter = Builders<ArrivalDTO>.Filter.Where(x => x.AirportPlaneStatus != AirportPlaneStatus.Finished.ToString());
            var result = await DbSet.Find(filter).ToListAsync(); 
            return result;
        }

        public async Task SetArrivalFinished(string planeId)
        {
            var res = await DbSet.FindOneAndUpdateAsync(
                Builders<ArrivalDTO>.Filter.Eq("PlaneId", planeId),
                Builders<ArrivalDTO>.Update.Set(p => p.AirportPlaneStatus, AirportPlaneStatus.Finished.ToString())
                );
        }
        public async Task SetArrivalStarted(string planeId)
        {
            var res = await DbSet.FindOneAndUpdateAsync(
                Builders<ArrivalDTO>.Filter.Eq("PlaneId", planeId),
                Builders<ArrivalDTO>.Update.Set(p => p.AirportPlaneStatus, AirportPlaneStatus.Started.ToString())
                );
        }
        public async Task<List<ArrivalDTO>> GetAllWaitingArrivals()
        {
            var filter = Builders<ArrivalDTO>.Filter.Where(x => x.AirportPlaneStatus == AirportPlaneStatus.Waiting.ToString());
            var result = await DbSet.Find(filter).ToListAsync();
            return result;
        }
        public async Task RemoveAllWaitingArrivals()
        {
            var filter = Builders<ArrivalDTO>.Filter.Where(x => x.AirportPlaneStatus == AirportPlaneStatus.Waiting.ToString());
            var result = await DbSet.DeleteManyAsync(filter);
        }
        
    }
}
