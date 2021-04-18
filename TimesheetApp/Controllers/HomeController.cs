using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimesheetApp.Models;
using TimesheetApp.ViewModels;

namespace TimesheetApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IWorkingWeekRepository _workingWeekRepository;
        private readonly IBillingCategoryRepository _billingCategory;
        private readonly IProjectRepository _project;
        private readonly IWorkingDayRepository _workingDay;
        private readonly IActivityRepository _activity;

        public HomeController(IBillingCategoryRepository billingCategoryRepository, IProjectRepository projectRepository, IWorkingDayRepository workingDayRepository, IActivityRepository activityRepository)
        {
            _billingCategory = billingCategoryRepository;
            _project = projectRepository;
            _workingDay = workingDayRepository;
            _activity = activityRepository;
        }
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                BillingCategories = _billingCategory.LoadAll(),
                Projects = _project.LoadAll(),
                WorkingDays = _workingDay.LoadCurrentWeek(),
                Activities = _activity.LoadAll(),
                DateName = Utilities.GetDaysName(),
                SimpleDate = Utilities.GetSimpleDate(Utilities.GetDaysOfCurrentWeek()),
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Submit(WorkingWeek workingWeek)
        {
            
            return View();
        }

        public IActionResult AddRow(WorkingWeek workingWeek)
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
