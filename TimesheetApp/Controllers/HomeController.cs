using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimesheetApp.Models;
using TimesheetApp.ViewModels;
using TimeSheetApp.Models;

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

        [TempData]
        public string ElementAttribute { get; set; }

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

            // Check total of Work hours is not less than 40 hour/week
            var total = 0;
            foreach (var hour in weekWorkHour)
            {
                total += int.Parse(hour);
            }

            weekIds.RemoveAt(0);

            for(var i = 0; i < weekIds.Count; i++)
            {
                var week = workingWeeks.Find(w => w.Id == ObjectId.Parse(weekIds[i]));

                // Check Project field is selected
                if(projects.Find(p => p.Id == ObjectId.Parse(weekProjects[i])) == null)
                {
                    Message = "Please select 'Project' and submit again.";
                    return RedirectToAction(nameof(Manage));
                }
                else
                {
                    week.Project = projects.Find(p => p.Id == ObjectId.Parse(weekProjects[i]));
                }

                // Check Activities field is selected
                if (activities.Find(a => a.Id == ObjectId.Parse(weekActivities[i])) == null)
                {
                    Message = "Please select 'Activity' and submit again.";
                    return RedirectToAction(nameof(Manage));
                }
                else
                {
                    week.Activity = activities.Find(a => a.Id == ObjectId.Parse(weekActivities[i]));
                }

                // Check Billing Categories field is selected
                if (billings.Find(b => b.Id == ObjectId.Parse(weekBillingCategory[i])) == null)
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

                weeks.Add(week);
            }

            foreach (var week in weeks)
            {
                _workingWeekRepository.Delete(week.Id);
                _workingWeekRepository.Create(week);
                _workingWeekRepository.SubmitToApprove(week);
            }

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult Manage()
        {
            _employeeId = ObjectId.Parse(HttpContext.Session.Get<string>("EmployeeId"));

            var workingWeeks = _workingWeekRepository.LoadWorkingWeekOfCurrentByEmployeeId(_employeeId);

            _workingWeeks = new WorkingWeekList
            {
                WorkingWeeks = workingWeeks.OrderBy(o => o.Order).ToList()
            };

            var lockId = new List<ObjectId>();
            _workingWeekRepository.LoadSubmitted(_employeeId, workingWeeks[0].From).ForEach(w => lockId.Add(w.Id));
            ViewBag.WeekLock = lockId;

            ViewBag.Permission = _employeeRepository.GetByObjectId(_employeeId).Role.Name;

            ViewBag.BillingCategory = _billingCategory.LoadAll();
            ViewBag.Project = _project.LoadAll();
            ViewBag.Activity = _activity.LoadAll();
            ViewBag.DayName = Utilities.GetDaysName();
            ViewBag.SimpleDate = Utilities.GetSimpleDate(Utilities.GetDaysOfCurrentWeek());

            return View(_workingWeeks);
        }
    }
}
