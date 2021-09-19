using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Models.DAL
{
    public class AirportDTO 
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId _id { get; set; }
        public List<StationDTO> DTOStations { get; set; }
        public List<int> ArrivalStartingStations { get; set; }
        public List<int> DepartureStartingStations { get; set; }
        public List<int> DepartureEndingStations { get; set; }
        public List<int> ArrivalEndingStations { get; set; }
        public DateTime CreatedAt => _id.CreationTime;
    }
}
