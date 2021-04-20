using MongoDB.Bson;
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
    public class ProjectRepository: BaseRepository, IProjectRepository
    {
        private readonly IMongoCollection<Project> _project;
        public ProjectRepository(IMongoClient client) : base(client)
        {
            var collection = database.GetCollection<Project>(jObject.SelectToken("projectCollection").ToString());

            _project = collection;
        }
        public IEnumerable<Project> LoadAll()
        {
            var project = _project.Find(_ => true).ToList();
            return project;
        }

        public void Create(Project project)
        {
            try
            {
                _project.InsertOne(project);
            }
            catch
            {

            }
        }

        public void Delete(ObjectId id)
        {
            try
            {
                var filter = Builders<Project>.Filter.Eq("Id", id);
                _project.DeleteOne(filter);
            }
            catch(Exception e)
            {

            }
        }
    }
}
