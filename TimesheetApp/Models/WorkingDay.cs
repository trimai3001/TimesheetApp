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
        public Project Project { get; set; }
        public BillingCategory BillingCategory { get; set; }
        public DateTime WorkDate { get; set; }
        public string Note { get; set; }
    }
}
