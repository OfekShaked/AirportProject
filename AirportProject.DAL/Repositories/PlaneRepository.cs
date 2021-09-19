using AirportProject.DAL.Interfaces;
using AirportProject.Models.DAL;
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
    }
}
