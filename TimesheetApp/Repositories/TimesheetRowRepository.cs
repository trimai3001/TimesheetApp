using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Interfaces;
using TimesheetApp.Models;

namespace TimesheetApp.Repositories
{
    public class TimesheetRowRepository : BaseRepository, ITimesheetRowRepository
    {
        private readonly IMongoCollection<TimesheetRow> _timesheetRow;
        public TimesheetRowRepository(IMongoClient client) : base(client)
        {
            var collection = database.GetCollection<TimesheetRow>(jObject.SelectToken("billingCategoryCollection").ToString());

            _timesheetRow = collection;
        }
        public IEnumerable<TimesheetRow> LoadAll()
        {
            var billingCategories = _timesheetRow.Find(_ => true).ToList();
            return billingCategories;
        }

        public IEnumerable<TimesheetRow> LoadTimesheetRowOfCurrentWeek()
        {
            var billingCategories = _timesheetRow.Find(_ => true).ToList();
            return billingCategories;
        }
    }
}
