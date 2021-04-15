using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TimeSheetApp.Models
{
    public class Employee
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Generator { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("role")]
        public Role Role { get; set; }

        [BsonElement("project")]
        public List<Project> Projects { get; set; }

        [BsonElement("activity")]
        public Activity Activity { get; set; }

        [BsonElement("billingCategory")]
        public BillingCategory BillingCategory { get; set; }

        [BsonElement("replaceFor")]
        public Employee ReplaceFor { get; set; }

        public Employee()
        {
        }
    }
}