using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimeSheetApp.Models;

namespace TimesheetApp.Repositories
{
    public class BillingCategoryRepository : BaseRepository, IBillingCategoryRepository
    {
        private readonly IMongoCollection<BillingCategory> _billingCategory;
        public BillingCategoryRepository(IMongoClient client) : base(client)
        {
            var collection = database.GetCollection<BillingCategory>(jObject.SelectToken("billingCategoryCollection").ToString());

            _billingCategory = collection;
        }
        public IEnumerable<BillingCategory> LoadAll()
        {
            var billingCategories = _billingCategory.Find(_ => true).ToList();
            return billingCategories;
        }
    }
}
