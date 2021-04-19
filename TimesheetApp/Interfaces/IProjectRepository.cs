using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Interfaces
{
    public interface IProjectRepository
    {
        public IEnumerable<Project> LoadAll();
        public void Create(Project project);
        public void Delete(ObjectId id);
    }
}
