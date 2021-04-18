using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Interfaces;
using TimeSheetApp.Models;

namespace TimesheetApp.Repositories
{
    public class ActivityRepository: BaseRepository, IActivityRepository 
    {
        private readonly IMongoCollection<Activity> _activity;
        public ActivityRepository(IMongoClient client): base(client)
        {
            var collection = database.GetCollection<Activity>(jObject.SelectToken("activityCollection").ToString());
            _activity = collection;
        }

        public IEnumerable<Activity> LoadAll()
        {
            var billingCategories = _activity.Find(_ => true).ToList();
            return billingCategories;
        }
    }
}
