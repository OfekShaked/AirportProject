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
    public class DepartureRepository : BaseRepository<DepartureDTO>,IDepartureRepository
    {
        public DepartureRepository(IMongoContext context):base(context)
        {

        }
        public async Task<DepartureDTO> AddDeparture(string planeId)
        {
            var newDeparture = new DepartureDTO { _id = ObjectId.GenerateNewId(), PlaneId = planeId, AirportPlaneStatus=AirportPlaneStatus.Waiting.ToString()};
            await DbSet.InsertOneAsync(newDeparture);
            return newDeparture;
        }

        public async Task<List<DepartureDTO>> GetAllFutureDepartures()
        {
            var filter = Builders<DepartureDTO>.Filter.Where(x => x.AirportPlaneStatus!=AirportPlaneStatus.Finished.ToString());
            var result = await DbSet.Find(filter).ToListAsync();
            return result;
        }
        public async Task<List<DepartureDTO>> GetAllWaitingDepartures()
        {
            var filter = Builders<DepartureDTO>.Filter.Where(x => x.AirportPlaneStatus == AirportPlaneStatus.Waiting.ToString());
            var result = await DbSet.Find(filter).ToListAsync();
            return result;
        }
        public async Task RemoveAllWaitingDepartures()
        {
            var filter = Builders<DepartureDTO>.Filter.Where(x => x.AirportPlaneStatus == AirportPlaneStatus.Waiting.ToString());
            var result = await DbSet.DeleteManyAsync(filter);
        }

        public async Task SetDepartureFinished(string planeId)
        {
            var res = await DbSet.FindOneAndUpdateAsync(
                Builders<DepartureDTO>.Filter.Eq("PlaneId", planeId),
                Builders<DepartureDTO>.Update.Set(p => p.AirportPlaneStatus, AirportPlaneStatus.Finished.ToString())
                );
        }
        public async Task SetDepartureStarted(string planeId)
        {
            var res = await DbSet.FindOneAndUpdateAsync(
                Builders<DepartureDTO>.Filter.Eq("PlaneId", planeId),
                Builders<DepartureDTO>.Update.Set(p => p.AirportPlaneStatus, AirportPlaneStatus.Started.ToString())
                );
        }
    }
}
