using AirportProject.DAL.Interfaces;
using AirportProject.Models.DAL;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Repositories
{
    public class AirportRepository : BaseRepository<AirportDTO>, IAirportRepository
    {
        public AirportRepository(IMongoContext context):base(context)
        {
          
        }

        public async Task AddOrUpdate(AirportDTO airport)
        {
            var airportFound = await GetById(airport._id.ToString());
                if (airportFound != null)
                {
                    await Update(airport);
                }
                else
                {
                    await Add(airport);
                }  
        }
    }
}
