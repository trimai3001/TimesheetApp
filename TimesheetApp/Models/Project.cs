using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TimeSheetApp.Models
{
    public class Project
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        [Required]
        public string Name { get; set; }

        [BsonElement("projectManager")]
        [Required]
        public Employee Manager { get; set; }
        public Project()
        {
            Id = ObjectId.GenerateNewId();
            Manager = new Employee();
        }
    }
}