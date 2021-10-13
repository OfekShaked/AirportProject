using AirportProject.Common.Enums;
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
    public class PlaneRepository: BaseRepository<PlaneDTO> ,IPlaneRepository
    {
        public PlaneRepository(IMongoContext context) : base(context)
        {

        }
        public async Task AddPlane(PlaneDTO plane)
        {
            await DbSet.InsertOneAsync(plane);
        }
        public async Task SetPlaneStatusFinished(string planeId)
        {
            var filter = Builders<PlaneDTO>.Filter.Where(plane => plane._id.Equals(planeId));
            var update = Builders<PlaneDTO>.Update
                        .Set(p => p.Status, PlaneStatus.Finished.ToString());
            var result = await DbSet.UpdateOneAsync(filter, update);
        }
    }
}
