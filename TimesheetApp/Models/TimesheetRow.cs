using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Models
{
    public class TimesheetRow
    {
        public ObjectId Id { get; set; }
        public IEnumerable<BillingCategory> BillingCategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<WorkingDay> WorkingDays { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public int Order { get; set; }

        public TimesheetRow()
        {

        }
    }
}
