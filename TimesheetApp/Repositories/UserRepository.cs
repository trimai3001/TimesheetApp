using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Interfaces;
using TimeSheetApp.Models;

namespace TimesheetApp.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly IMongoCollection<User> _user;
        public UserRepository(IMongoClient client) : base(client)
        {
            var collection = database.GetCollection<User>(jObject.SelectToken("userCollection").ToString());

            _user = collection;
        }

        public IEnumerable<User> LoadAll()
        {
            var users = _user.Find(_ => true).ToList();
            return users;
        }
    }
}
