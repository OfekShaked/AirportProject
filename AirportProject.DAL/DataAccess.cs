using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL
{
    public class DataAccess
    {
        public IAirportRepository AirportRepository { get; set; }
        public IPlaneChangesRepository PlaneChangesRepository { get; set; }
        public IPlaneRepository PlaneRepository { get; set; }
        public IStationRepository StationRepository { get; set; }
        public IArrivalRepository ArrivalRepository { get; set; }
        public IDepartureRepository DepartureRepository { get; set; }
        IMongoContext _contex;
        public DataAccess(IMongoContext context)
        {
            _contex = context;
            AirportRepository = new AirportRepository(context);
            PlaneRepository = new PlaneRepository(context);
            StationRepository = new StationRepository(context);
            PlaneChangesRepository = new PlaneChangesRepository(context);
            DepartureRepository = new DepartureRepository(context);
            ArrivalRepository = new ArrivalRepository(context);
        }
        public string GetNewObjectId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
