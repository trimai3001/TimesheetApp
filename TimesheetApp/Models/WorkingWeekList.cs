using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;

namespace TimesheetApp.Models
{
    public class WorkingWeekList
    {
        public List<WorkingWeek> WorkingWeeks { get; set; }
    }
}
