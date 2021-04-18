using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Interfaces
{
    public interface IActivityRepository
    {
        public IEnumerable<Activity> LoadAll();
    }
}
