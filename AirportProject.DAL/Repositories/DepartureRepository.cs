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
    public class DepartureRepository : BaseRepository<DepartureDTO>,IDepartureRepository
    {
        public DepartureRepository(IMongoContext context):base(context)
        {

        }
        public Task AddDeparture(string planeId)
        {
            return Add(new DepartureDTO { PlaneId = planeId, IsFinished = false });
        }

        public async Task SetDepartureFinished(string planeId)
        {
            var data = await DbSet.FindAsync(Builders<DepartureDTO>.Filter.Eq("PlaneId", planeId));
            var planeFound = data.FirstOrDefault();
            planeFound.IsFinished = true;
            await Update(planeFound);
        }
    }
}
