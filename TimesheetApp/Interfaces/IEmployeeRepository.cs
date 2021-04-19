using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Interfaces
{
    public interface IEmployeeRepository
    {
        public IEnumerable<Employee> LoadAll();
        public void CreateEmployee(Employee employee);
        public void DeleteEmployee(int employeeId);
        public void DeleteEmployee(ObjectId id);
        public string GenerateEmployeeId();
    }
}
