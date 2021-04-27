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
        public List<WorkingWeek> LoadSubmitted(ObjectId employeeId, DateTime from);
        public WorkingWeek LoadSubmitted(ObjectId id);
        public List<WorkingWeek> LoadSubmittedByManager(ObjectId managerId);
        public List<WorkingWeek> LoadApprovedByManager(ObjectId managerId);
        public void Approve(ObjectId id);
        public void DeleteSubmitted(ObjectId id);
        public List<WorkingWeek> LoadApproved(ObjectId employeeId, DateTime from);
    }
}
