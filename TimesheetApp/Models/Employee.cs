using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using TimesheetApp.Helper;

namespace TimeSheetApp.Models
{
    public class Employee
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("employeeId")]
        public string EmployeeId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("role")]
        public Role Role { get; set; }
        public Employee()
        {
            Id = ObjectId.GenerateNewId();
            Role = new Role();
        }
    }
}