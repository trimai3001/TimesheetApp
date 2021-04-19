using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Name { get; set; }

        [BsonElement("email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Required]
        public string Email { get; set; }

        [BsonElement("role")]
        [Required]
        public Role Role { get; set; }
        public Employee()
        {
            Id = ObjectId.GenerateNewId();
            EmployeeId = "0";
            Role = new Role();
            Email = "";
        }
    }
}