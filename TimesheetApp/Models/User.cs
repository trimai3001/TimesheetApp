using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TimeSheetApp.Models
{
    public class User
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonRequired]
        [StringLength(50, MinimumLength = 3)]
        [BsonElement("username")]
        public int Username { get; set; }

        [BsonRequired]
        [StringLength(50, MinimumLength = 3)]
        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("employeeId")]
        public string EmployeeId { get; set; }

        public User()
        {
        }
    }
}