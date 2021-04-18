using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimesheetApp.Models;

namespace TimesheetApp.Repositories
{
    public class WorkingWeekRepository : IWorkingWeekRepository
    {
        private readonly IMongoCollection<WorkingWeek> _workingWeeks;

        public IEnumerable<WorkingWeek> LoadAll()
        {
            var workingWeek = _workingWeeks.Find(_ => true).ToList();
            return workingWeek;
        }

        public IEnumerable<WorkingWeek> LoadWorkingWeekOfCurrent()
        {
            var monday = Utilities.GetMonday(DateTime.Today);
            var workingWeeks = _workingWeeks.Find(w => w.From == monday).ToList();

            if(workingWeeks.Count() == 0)
            {
                var workingWeek = new WorkingWeek();
            }

            return workingWeeks;
        }

        public IEnumerable<WorkingWeek> LoadWorkingWeekOfCurrentByEmployeeId(ObjectId employeeId)
        {
            var monday = Utilities.GetMonday(DateTime.Today);
            var workingWeeks = _workingWeeks.Find(w => w.From == monday && w.Employee.Id == employeeId).ToList();

            if (workingWeeks.Count() == 0)
            {
                var workingWeek = new WorkingWeek();
            }

            return workingWeeks;
        }

        
    }
}
