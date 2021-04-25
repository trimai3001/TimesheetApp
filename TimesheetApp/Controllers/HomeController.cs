using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        private WorkingWeekList _workingWeeks;
        private ObjectId _employeeId;

        [TempData]
        public string Message { get; set; }

        public HomeController(IBillingCategoryRepository billingCategoryRepository, IProjectRepository projectRepository, IActivityRepository activityRepository, IWorkingWeekRepository workingWeekRepository)
        {
            _billingCategory = billingCategoryRepository;
            _project = projectRepository;
            _activity = activityRepository;
            _workingWeekRepository = workingWeekRepository;

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
            _employeeId = ObjectId.Parse(HttpContext.Session.Get<string>("EmployeeId"));
            var workingWeeks = _workingWeekRepository.LoadWorkingWeekOfCurrentByEmployeeId(_employeeId);

            if (workingWeeks.Find(s => s.Project.Name == null) != null)
            {
                Message = "Please select 'Project' and submit again.";
                
                //return RedirectToAction(nameof(Manage));
            }
            else if (workingWeeks.Find(s => s.BillingCategory.Name == null) != null)
            {
                Message = "Please select 'Billing Category' and submit again.";
                //return RedirectToAction(nameof(Manage));
            }
            else if (workingWeeks.Find(s => s.Activity.Name == null) != null)
            {
                Message = "Please select 'Billing Category' and submit again.";
            }
            else
            {
                foreach (var week in workingWeeks)
                {
                    _workingWeekRepository.SubmitToApprove(week);
                }
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
            
            ViewBag.BillingCategory = _billingCategory.LoadAll();
            ViewBag.Project = _project.LoadAll();
            ViewBag.Activity = _activity.LoadAll();
            ViewBag.DayName = Utilities.GetDaysName();
            ViewBag.SimpleDate = Utilities.GetSimpleDate(Utilities.GetDaysOfCurrentWeek());

            return View(_workingWeeks);
        }
    }
}
