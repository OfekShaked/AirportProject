using AirportProject.Common.Enums;
using AirportProject.DAL.Interfaces;
using AirportProject.Models.DAL;
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

        public async Task SetPlaneStatusFinished(string planeId)
        {
            var filter = Builders<PlaneDTO>.Filter.Where(plane => plane._id == new ObjectId(planeId));
            var update = Builders<PlaneDTO>.Update
                        .Set(p => p.Status, PlaneStatus.Finished);
            var result = await DbSet.UpdateOneAsync(filter, update);
        }
    }
}
