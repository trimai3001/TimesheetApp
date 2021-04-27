using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimesheetApp.Models;

namespace TimesheetApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWorkingWeekRepository _workingWeekRepository;
        private readonly IBillingCategoryRepository _billingCategory;
        private readonly IProjectRepository _project;
        private readonly IActivityRepository _activity;
        private readonly IEmployeeRepository _employeeRepository;

        private WorkingWeekList _workingWeeks;
        private ObjectId _employeeId;

        [TempData]
        public string Message { get; set; }

        public HomeController(IBillingCategoryRepository billingCategoryRepository, IProjectRepository projectRepository, IActivityRepository activityRepository, IWorkingWeekRepository workingWeekRepository, IEmployeeRepository employeeRepository)
        {
            _billingCategory = billingCategoryRepository;
            _project = projectRepository;
            _activity = activityRepository;
            _workingWeekRepository = workingWeekRepository;
            _employeeRepository = employeeRepository;
            ViewBag.Message = "";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddRow()
        {
            _employeeId = ObjectId.Parse(HttpContext.Session.Get<string>("EmployeeId"));
            var week = new WorkingWeek(_employeeId);
            var workingWeeks = _workingWeekRepository.LoadWorkingWeekOfCurrentByEmployeeId(_employeeId);
            week.Order = workingWeeks.Last().Order + 1;
            _workingWeekRepository.Create(week);

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(IFormCollection form)
        {
            _employeeId = ObjectId.Parse(HttpContext.Session.Get<string>("EmployeeId"));
            ObjectId id = ObjectId.Parse(form["week.Id"].ToString());
            _workingWeekRepository.Delete(id);

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(IFormCollection form)
        {

            var weeks = new List<WorkingWeek>();

            // Load data
            _employeeId = ObjectId.Parse(HttpContext.Session.Get<string>("EmployeeId"));
            var workingWeeks = _workingWeekRepository.LoadWorkingWeekOfCurrentByEmployeeId(_employeeId);
            var projects = _project.LoadAll().ToList();
            var activities = _activity.LoadAll().ToList();
            var billings = _billingCategory.LoadAll().ToList();

            // Get all id of weeks
            var weekIds = form["week.Id"].ToString().Split(",").ToList();
            var weekProjects = form["Project"].ToString().Split(",").ToList();
            var weekActivities = form["Activity"].ToString().Split(",").ToList();
            var weekBillingCategory = form["BillingCategory"].ToString().Split(",").ToList();
            var weekWorkHour = form["WorkHour"].ToString().Split(",").ToList();
            var monday = Utilities.GetMonday(DateTime.Today);

            weekIds.RemoveAt(0);

            for(var i = weekIds.Count - 1; i >= 0; i--)
            {
                if (_workingWeekRepository.LoadSubmitted(ObjectId.Parse(weekIds[i])) != null)
                {
                    weekIds.RemoveAt(i);
                }
            }

            if(weekIds.Count == 0)
            {
                Message = "Your timesheet already submitted";
                return RedirectToAction(nameof(Manage));
            }

            // Check total of Work hours is not less than 40 hour/week
            var total = 0;
            foreach (var hour in weekWorkHour)
            {
                total += int.Parse(hour);
            }

            

            for(var i = 0; i < weekIds.Count; i++)
            {
                var week = workingWeeks.Find(w => w.Id == ObjectId.Parse(weekIds[i]));

                // Check Project field is selected
                if(weekProjects[i] == "" || projects.Find(p => p.Id == ObjectId.Parse(weekProjects[i])) == null)
                {
                    Message = "Please select 'Project' and submit again.";
                    return RedirectToAction(nameof(Manage));
                }
                else
                {
                    week.Project = projects.Find(p => p.Id == ObjectId.Parse(weekProjects[i]));
                }

                // Check Activities field is selected
                if (weekActivities[i] == "" || activities.Find(a => a.Id == ObjectId.Parse(weekActivities[i])) == null)
                {
                    Message = "Please select 'Activity' and submit again.";
                    return RedirectToAction(nameof(Manage));
                }
                else
                {
                    week.Activity = activities.Find(a => a.Id == ObjectId.Parse(weekActivities[i]));
                }

                // Check Billing Categories field is selected
                if (weekBillingCategory[i] == "" || billings.Find(b => b.Id == ObjectId.Parse(weekBillingCategory[i])) == null)
                {
                    Message = "Please select 'Billing Category' and submit again.";
                    return RedirectToAction(nameof(Manage));
                }
                else
                {
                    week.BillingCategory = billings.Find(b => b.Id == ObjectId.Parse(weekBillingCategory[i]));
                }

                if (total < 40)
                {
                    Message = "Your hours you work are less than 40hrs. Please submit again.";
                    return RedirectToAction(nameof(Manage));
                }

                
                var days = week.WorkingDays;
                var index = 0;
                foreach (var day in days)
                {
                    if(index == 7)
                    {
                        break;
                    }
                    day.WorkHour = int.Parse(weekWorkHour[index + (7 * i)]);
                    day.WorkDate = monday.AddDays(index);
                    index++;
                }
                week.WorkingDays = days;
                week.From = monday;
                week.To = monday.AddDays(6);
                week.Order = i;
                weeks.Add(week);
            }

            // Check submitted working week
            var locks = _workingWeekRepository.LoadSubmitted(_employeeId, monday);
            locks.AddRange(_workingWeekRepository.LoadApproved(_employeeId, monday));

            foreach(var week in weeks)
            {
                if(locks.Count == 0 || !locks.Contains(week))
                {
                    _workingWeekRepository.Delete(week.Id);
                    _workingWeekRepository.Create(week);
                    _workingWeekRepository.SubmitToApprove(week);
                }
                else if(locks.Contains(week))
                {
                    continue;
                }
            }

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult Manage()
        {
            // Init
            _employeeId = ObjectId.Parse(HttpContext.Session.Get<string>("EmployeeId"));
            ViewBag.Permission = _employeeRepository.GetByObjectId(_employeeId).Role.Name;
            ViewBag.BillingCategory = _billingCategory.LoadAll();
            ViewBag.Project = _project.LoadAll();
            ViewBag.Activity = _activity.LoadAll();
            ViewBag.DayName = Utilities.GetDaysName();
            ViewBag.SimpleDate = Utilities.GetSimpleDate(Utilities.GetDaysOfCurrentWeek());
            var monday = Utilities.GetMonday(DateTime.Today);

            // Load from submitted and approved
            var workingWeeks = _workingWeekRepository.LoadSubmitted(_employeeId, monday);
            workingWeeks.AddRange(_workingWeekRepository.LoadApproved(_employeeId, monday));

            ViewBag.WeekLock = workingWeeks.ToList();

            var ids = workingWeeks.Select(w => w.Id);

            // Load from db
            var workingWeeksDB = _workingWeekRepository.LoadWorkingWeekOfCurrentByEmployeeId(_employeeId);
            foreach(var workWeek in workingWeeksDB)
            {
                if(!ids.Contains(workWeek.Id))
                {
                    workingWeeks.Add(workWeek);
                }
            }

            // Load Model to View
            _workingWeeks = new WorkingWeekList
            {
                WorkingWeeks = workingWeeks
            };

            return View(_workingWeeks);
        }
    }
}
