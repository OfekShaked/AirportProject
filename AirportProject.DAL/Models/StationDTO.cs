using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Models
{
    public class StationDTO 
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        [BsonElement]
        public PlaneDTO CurrentPlaneInside { get; set; }
        [BsonElement]
        public string CurrentPlaneIdInside { get; set; }
        public TimeSpan HandlingTime { get; set; }
        public List<int> ConnectedDepartureStations { get; set; }
        public List<int> ConnectedArrivalStations { get; set; }
        public DateTime CreatedAt => _id.CreationTime;
    }
}
