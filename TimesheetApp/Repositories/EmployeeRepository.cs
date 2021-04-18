using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
