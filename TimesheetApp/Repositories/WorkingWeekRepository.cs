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
        private readonly IMongoCollection<WorkingWeek> _workingWeek;

        public IEnumerable<WorkingWeek> Get()
        {
            var workingWeek = _workingWeek.Find(_ => true).ToList();
            return workingWeek;
        }
    }
}
