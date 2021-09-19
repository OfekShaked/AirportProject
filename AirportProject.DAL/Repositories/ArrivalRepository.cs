using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Models;
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

        public Task AddArrival(string planeId)
        {
            return Add(new ArrivalDTO { PlaneId = planeId, IsFinished = false });
        }

        public async Task SetArrivalFinished(string planeId)
        {
            var data = await DbSet.FindAsync(Builders<ArrivalDTO>.Filter.Eq("PlaneId", planeId));
            var planeFound = data.FirstOrDefault();
            planeFound.IsFinished = true;
            await Update(planeFound);
        }
    }
}
