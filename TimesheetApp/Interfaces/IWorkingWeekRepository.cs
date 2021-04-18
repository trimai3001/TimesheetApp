using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Models;

namespace TimesheetApp.Interfaces
{
    interface IWorkingWeekRepository
    {
        public IEnumerable<WorkingWeek> LoadAll();
        public IEnumerable<WorkingWeek> LoadWorkingWeekOfCurrent();
    }
}
