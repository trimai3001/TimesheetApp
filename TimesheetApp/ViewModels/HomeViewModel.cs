using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BillingCategory> BillingCategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }


        public HomeViewModel()
        {

        }
    }
}
