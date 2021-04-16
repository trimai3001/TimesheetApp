using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Models
{
    public class WorkingWeek
    {
        public string Id { get; set; }
        public Employee Employee { get; set; }
        public List<WorkingDay> WorkingDays { get; set; }
    }
}
