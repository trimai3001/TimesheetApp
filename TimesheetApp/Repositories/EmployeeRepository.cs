using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using TimesheetApp.Interfaces;
using TimeSheetApp.Models;

namespace TimesheetApp.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employee;
        public EmployeeRepository(IMongoClient client) : base(client)
        {
            var collection = database.GetCollection<Employee>(jObject.SelectToken("employeeCollection").ToString());

            _employee = collection;
        }
        public IEnumerable<Employee> LoadAll()
        {
            var employee = _employee.Find(_ => true).ToList();
            return employee;
        }

        public void CreateEmployee(Employee employee)
        {
            try
            {
                _employee.InsertOne(employee);
            }
            catch
            {

            }
        }

        public void DeleteEmployee(int employeeId) {
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", employeeId);
            _employee.DeleteOne(filter);
        }

        public void DeleteEmployee(ObjectId id)
        {
            var filter = Builders<Employee>.Filter.Eq("Id", id);
            _employee.DeleteOne(filter);
        }
    }
}
