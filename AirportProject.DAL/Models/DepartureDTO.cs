using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Models
{
    public class DepartureDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId _id { get; set; }
        public string PlaneId { get; set; }
        public string AirportPlaneStatus { get; set; }
        public DateTime CreatedAt => _id.CreationTime;
    }
}
