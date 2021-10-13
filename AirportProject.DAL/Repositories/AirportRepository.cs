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
    public class AirportRepository : BaseRepository<AirportDTO>, IAirportRepository
    {
        public AirportRepository(IMongoContext context):base(context)
        {
          
        }
        public async Task<AirportDTO> GetAirport(string id = null)
        {
            if (id == null)
            {
                var airportsFound = await GetAll();
                return airportsFound.FirstOrDefault();
            }
            else
            {
                return await GetById(id);
            }
        }
        public async Task UpdateAirport(AirportDTO airport)
        {
            var filter = Builders<AirportDTO>.Filter.Eq("_id", airport._id);
            var update = Builders<AirportDTO>.Update.Set(a => a.DTOStations, airport.DTOStations);
            var result = await DbSet.UpdateOneAsync(filter, update);
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
