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
    public class WorkingWeekRepository : BaseRepository, IWorkingWeekRepository
    {
        private readonly IMongoCollection<WorkingWeek> _workingWeek;
        public WorkingWeekRepository(IMongoClient client) : base(client)
        {
            var collection = database.GetCollection<WorkingWeek>(jObject.SelectToken("workingWeekCollection").ToString());

            _workingWeek = collection;
        }

        public IEnumerable<WorkingWeek> LoadAll()
        {
            var workingWeek = _workingWeek.Find(_ => true).ToList();
            return workingWeek;
        }

        public List<WorkingWeek> LoadWorkingWeekOfCurrentByEmployeeId(ObjectId employeeId)
        {
            var workingWeeks = new List<WorkingWeek>();
            var monday = Utilities.GetMonday(DateTime.Today);
            workingWeeks = _workingWeek.Find(w => w.From == monday && w.EmployeeId == employeeId).ToList();

            if (workingWeeks.Count() == 0)
            {
                var workingWeek = new WorkingWeek(employeeId);
                workingWeeks.Add(workingWeek);
                Create(workingWeek);
            }


            return workingWeeks;
        }

        public void Create(WorkingWeek workingWeek)
        {
            _workingWeek.InsertOne(workingWeek);
        }

        public void Delete(ObjectId id)
        {
            var filter = Builders<WorkingWeek>.Filter.Eq("Id", id);
            _workingWeek.DeleteOne(filter);
        }

        public void SubmitToApprove(WorkingWeek workingWeek)
        {
            var toApproveCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApproveCollection").ToString());
            toApproveCollection.InsertOne(workingWeek);
        }

        public List<WorkingWeek> LoadSubmitted(ObjectId employeeId, DateTime from)
        {
            var workingWeek = _workingWeek.Find(w => w.EmployeeId == employeeId && w.From == from).ToList();
            return workingWeek;
        }
    }
}
