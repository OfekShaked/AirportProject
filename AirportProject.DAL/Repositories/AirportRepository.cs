using AirportProject.DAL.Interfaces;
using AirportProject.Models.DAL;
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
    }
}
