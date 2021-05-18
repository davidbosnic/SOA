using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorDataService.Model
{
    public class SensorDataModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("value")]
        public string Value { get; set; }
        [BsonElement("recordTime")]
        public string RecordTime { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
    }
}
