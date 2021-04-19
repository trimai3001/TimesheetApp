using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Interfaces;
using TimeSheetApp.Models;

namespace TimesheetApp.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        private readonly IMongoCollection<Role> _role;
        public RoleRepository(IMongoClient client) : base(client)
        {
            var collection = database.GetCollection<Role>(jObject.SelectToken("roleCollection").ToString());

            _role = collection;
        }

        public Role GetRoleById(ObjectId objectId)
        {
            var role = _role.Find(r => r.Id == objectId).FirstOrDefault();
            return role;
        }

        public IEnumerable<Role> LoadAll()
        {
            var roles = _role.Find(_ => true).ToList();
            return roles;
        }
    }
}
