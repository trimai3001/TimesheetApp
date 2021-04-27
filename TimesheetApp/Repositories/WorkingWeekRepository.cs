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
            workingWeeks = _workingWeek.Find(w => w.EmployeeId == employeeId).ToList();
            workingWeeks = workingWeeks.FindAll(w => w.From.Day == monday.Day && w.From.Month == monday.Month && w.From.Year == monday.Year);

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
            var monday = Utilities.GetMonday(DateTime.Today);
            monday = monday.AddDays(1);
            workingWeek.From = monday;
            workingWeek.To = monday.AddDays(6);
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
            var monday = Utilities.GetMonday(DateTime.Today);
            monday = monday.AddDays(1);
            workingWeek.From = monday;
            workingWeek.To = monday.AddDays(6);
            toApproveCollection.InsertOne(workingWeek);
        }

        public List<WorkingWeek> LoadSubmitted(ObjectId employeeId, DateTime from)
        {
            var toApproveCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApproveCollection").ToString());
            var workingWeek = toApproveCollection.Find(w => w.EmployeeId == employeeId).ToList();

            workingWeek = workingWeek.FindAll(w => w.From.Day == from.Day && w.From.Month == from.Month && w.From.Year == from.Year);
            return workingWeek;
        }

        public WorkingWeek LoadSubmitted(ObjectId id)
        {
            var toApproveCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApproveCollection").ToString());
            var workingWeek = toApproveCollection.Find(w => w.Id == id).CountDocuments() == 0 ? null : toApproveCollection.Find(w => w.Id == id).FirstOrDefault();
            return workingWeek;
        }

        public List<WorkingWeek> LoadApproved(ObjectId employeeId, DateTime from)
        {
            var approvedCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApprovedCollection").ToString());
            var workingWeek = approvedCollection.Find(w => w.EmployeeId == employeeId).ToList();
            workingWeek = workingWeek.FindAll(w => w.From.Day == from.Day && w.From.Month == from.Month && w.From.Year == from.Year);
            return workingWeek;
        }

        public List<WorkingWeek> LoadSubmittedByManager(ObjectId managerId)
        {
            var toApproveCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApproveCollection").ToString());
            var workingWeeks = toApproveCollection.Find(w => w.Project.Manager.Id == managerId).CountDocuments() == 0 ? null : toApproveCollection.Find(w => w.Project.Manager.Id == managerId).ToList();

            return workingWeeks;
        }

        public List<WorkingWeek> LoadApprovedByManager(ObjectId managerId)
        {
            var approvedCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApprovedCollection").ToString());
            var workingWeeks = approvedCollection.Find(w => w.Project.Manager.Id == managerId).CountDocuments() == 0 ? null : approvedCollection.Find(w => w.Project.Manager.Id == managerId).ToList();

            return workingWeeks;
        }

        public void Approve(ObjectId id)
        {
            var week = LoadSubmitted(id);
            var approvedCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApprovedCollection").ToString());
            approvedCollection.InsertOne(week);
        }

        public void DeleteSubmitted(ObjectId id)
        {
            var filter = Builders<WorkingWeek>.Filter.Eq("Id", id);
            var toApproveCollection = database.GetCollection<WorkingWeek>(jObject.SelectToken("ApproveCollection").ToString());
            toApproveCollection.DeleteOne(filter);
        }
    }
}
