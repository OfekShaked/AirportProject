using AirportProject.Commom.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Models
{
    public class PlaneChangesDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId _id { get; set; }
        public string PlaneId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public PlaneStationStatus PlaneStationStatus { get; set; }
        public DateTime CreatedAt => _id.CreationTime;
    }
}
