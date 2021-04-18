using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.ViewModels
{
    public class EmployeeViewModel
    {
        public Employee _employee;
        public IEnumerable<Role> _roles;
    }
}
