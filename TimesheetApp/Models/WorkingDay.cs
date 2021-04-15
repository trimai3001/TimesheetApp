using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Models
{
    public class WorkingDay
    {
        public string Id { get; set; }
        public Project project { get; set; }
        public DateTime WorkDate { get; set; }
        public string Note { get; set; }
    }
}
