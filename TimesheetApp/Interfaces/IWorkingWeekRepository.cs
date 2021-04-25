using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Models;

namespace TimesheetApp.Interfaces
{
    public interface IWorkingWeekRepository
    {
        public IEnumerable<WorkingWeek> LoadAll();
        public List<WorkingWeek> LoadWorkingWeekOfCurrentByEmployeeId(ObjectId employeeId);
        public void Create(WorkingWeek workingWeek);
        public void Delete(ObjectId id);
        public void SubmitToApprove(WorkingWeek workingWeek);
    }
}
