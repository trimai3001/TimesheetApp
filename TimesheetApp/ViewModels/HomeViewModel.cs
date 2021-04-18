using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Models;
using TimeSheetApp.Models;

namespace TimesheetApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BillingCategory> BillingCategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<WorkingDay> WorkingDays { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public IEnumerable<string> DateName { get; set; }
        public IEnumerable<string> SimpleDate { get; set; }
        public IEnumerable<WorkingWeek> WorkingWeeks { get; set;} 
        public HomeViewModel()
        {

        }
    }
}
