using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Models
{
    public class WorkingWeek
    {
        public ObjectId Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<WorkingDay> WorkingDays { get; set; }
    }
}
