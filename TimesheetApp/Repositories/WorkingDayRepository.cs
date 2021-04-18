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
    public class WorkingDayRepository : BaseRepository, IWorkingDayRepository
    {
        private readonly IMongoCollection<WorkingDay> _workingDay;

        public WorkingDayRepository(IMongoClient client): base(client)
        {
            var collection = database.GetCollection<WorkingDay>(jObject.SelectToken("workingDayCollection").ToString());
            _workingDay = collection;
        }

        public IEnumerable<WorkingDay> LoadAll()
        {
            var workingDay = _workingDay.Find(_ => true).ToList();
            return workingDay;
        }

        public IEnumerable<WorkingDay> LoadCurrentWeek()
        {
            var workingDaysLoaded = LoadAll();
            var dayOfCurrentWeek = Utilities.GetDaysOfCurrentWeek();
            var workingDaysOfCurrentWeek = new List<WorkingDay>();

            dayOfCurrentWeek.ToList().ForEach(day =>
            {
                var list = workingDaysLoaded.ToList();
                var workDay = new WorkingDay();

                var find = list.Find(d => d.WorkDate == day);

                if (find == null)
                {
                    workDay.WorkDate = day;
                }
                else
                {
                    workDay = find;
                }

                workingDaysOfCurrentWeek.Add(workDay);
            });

            return workingDaysOfCurrentWeek;
        }

        public IEnumerable<WorkingDay> LoadCurrentWeekByEmployeeId(ObjectId employeeId)
        {
            var workingDaysLoaded = LoadAll();
            var dayOfCurrentWeek = Utilities.GetDaysOfCurrentWeek();
            var workingDaysOfCurrentWeek = new List<WorkingDay>();

            dayOfCurrentWeek.ToList().ForEach(day =>
            {
                var list = workingDaysLoaded.ToList();
                var workDay = new WorkingDay(employeeId);

                var find = list.Find(d => d.WorkDate == day);

                if (find == null)
                {
                    workDay.WorkDate = day;
                }
                else
                {
                    workDay = find;
                }

                workingDaysOfCurrentWeek.Add(workDay);
            });

            return workingDaysOfCurrentWeek;
        }
    }
}
