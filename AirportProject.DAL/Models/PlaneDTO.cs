﻿using AirportProject.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirportProject.DAL.Models
{
    public class PlaneDTO 
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId _id { get; set; }
        public string Name { get; set; }        
        public string Status { get; set; }
        [BsonElement]
        public int CurrentStationId { get; set; }
        public DateTime CreatedAt => _id.CreationTime;
    }
}
