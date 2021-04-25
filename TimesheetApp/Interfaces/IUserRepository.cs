using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Interfaces
{
    public interface IUserRepository
    {
        public IEnumerable<User> LoadAll();
    }
}
