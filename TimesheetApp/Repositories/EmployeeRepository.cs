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
            try
            {
                var filter = Builders<Employee>.Filter.Eq("Id", id);
                _employee.DeleteOne(filter);
            }
            catch
            {

            }
        }

        public string GenerateEmployeeId()
        {
            string generate = "0";
            var employees = LoadAll();
            if (employees.Count() == 0 || employees.First().EmployeeId == null)
            {
                return generate;
            }
            else
            {
                var id = int.Parse(employees.ElementAt(employees.Count() - 1).EmployeeId) + 1;
                generate = id.ToString();
            }
            
            
            return generate;
        }

        public Employee GetByObjectId(ObjectId objectId)
        {
            var employee = _employee.Find(e => e.Id == objectId).FirstOrDefault();
            return employee;
        }

        public IEnumerable<Employee> GetAllByRole(string role)
        {
            var employee = _employee.Find(e => e.Role.Name == role).ToList();
            return employee;
        }
    }
}
