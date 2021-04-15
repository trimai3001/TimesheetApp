using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimesheetApp.Models
{
    public class WorkingWeek
    {
        public string Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public List<WorkingDay> WorkingDays { get; set; }
    }
}
