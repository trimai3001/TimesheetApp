using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Interfaces
{
    public interface IRoleRepository
    {
        public IEnumerable<Role> LoadAll();
        public Role GetRoleById(ObjectId objectId);
    }
}
