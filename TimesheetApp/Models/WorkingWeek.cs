using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
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

        [BsonElement("employee")]
        public Employee Employee { get; set; }

        [BsonElement("project")]
        public Project Project { get; set; }
        [BsonElement("activity")]
        public Activity Activity { get; set; }
        [BsonElement("billingCategory")]
        public BillingCategory BillingCategory { get; set; }
        [BsonElement("from")]
        public DateTime From { get; set; }
        [BsonElement("to")]
        public DateTime To { get; set; }
        [BsonElement("workingDays")]
        public List<WorkingDay> WorkingDays { get; set; }
        [BsonElement("order")]
        public int Order { get; set; }

        public WorkingWeek()
        {
            Id = ObjectId.GenerateNewId();
            Employee = new Employee();
            Project = new Project();
            Activity = new Activity();
            BillingCategory = new BillingCategory();
            From = Utilities.GetMonday(DateTime.Today);
            To = From.AddDays(6);
            WorkingDays = new List<WorkingDay>();
            for (var i = 0; i < 7; i++)
            {
                var workingDay = new WorkingDay();
                WorkingDays.Add(workingDay);
            }
            Order = 1;
        }
    }
}
