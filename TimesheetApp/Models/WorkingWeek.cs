using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;
using TimeSheetApp.Models;

namespace TimesheetApp.Models
{
    public class WorkingWeek
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [Required]
        [BsonElement("project")]
        public Project Project { get; set; }

        [Required]
        [BsonElement("activity")]
        public Activity Activity { get; set; }

        [Required]
        [BsonElement("billingCategory")]
        public BillingCategory BillingCategory { get; set; }

        [BsonElement("employeeId")]
        public ObjectId EmployeeId {get; set; }

        [BsonElement("from")]
        public DateTime From { get; set; }

        [BsonElement("to")]
        public DateTime To { get; set; }

        [BsonElement("workingDays")]
        public List<WorkingDay> WorkingDays { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }

        public WorkingWeek(ObjectId employeeId)
        {
            Id = ObjectId.GenerateNewId();
            Project = new Project();
            Activity = new Activity();
            BillingCategory = new BillingCategory();
            EmployeeId = employeeId;

            From = Utilities.GetMonday(DateTime.Today);
            To = From.AddDays(6);

            WorkingDays = new List<WorkingDay>();

            for (var i = 0; i < 7; i++)
            {
                var workingDay = new WorkingDay(Id);
                WorkingDays.Add(workingDay);
            }

            Order = 1;
        }
    }
}
