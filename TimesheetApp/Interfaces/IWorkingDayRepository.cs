using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Models;

namespace TimesheetApp.Interfaces
{
    public interface IWorkingDayRepository
    {
        public IEnumerable<WorkingDay> LoadAll();

        public IEnumerable<WorkingDay> LoadCurrentWeek();
    }
}
