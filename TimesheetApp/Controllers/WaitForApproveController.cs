using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimesheetApp.Models;
using TimeSheetApp.Models;

namespace TimesheetApp.Controllers
{
    public class WaitForApproveController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkingWeekRepository _workingWeekRepository;

        private ObjectId _employeeId;
        public WaitForApproveController(IEmployeeRepository employeeRepository, IWorkingWeekRepository workingWeekRepository)
        {
            _employeeRepository = employeeRepository;
            _workingWeekRepository = workingWeekRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(IFormCollection form)
        {
            var weekIds = form["objectsId"].ToString();
            weekIds = weekIds.Substring(0, weekIds.Length - 1);
            var ids = weekIds.Split(",");
            for (var i = 0; i < ids.Count(); i++)
            {
                _workingWeekRepository.DeleteSubmitted(ObjectId.Parse(ids[i]));
            }

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(IFormCollection form)
        {
            var weekIds = form["objectsId"].ToString();
            weekIds = weekIds.Substring(0, weekIds.Length - 1);
            var ids = weekIds.Split(",");
            for(var i = 0; i < ids.Count(); i++)
            {
                _workingWeekRepository.Approve(ObjectId.Parse(ids[i]));
                _workingWeekRepository.DeleteSubmitted(ObjectId.Parse(ids[i]));
            } 

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult Manage()
        {
            // Init
            var employeeId = new List<ObjectId>();
            var employeeObjects = new List<Employee>();

            // Check permission
            _employeeId = ObjectId.Parse(HttpContext.Session.Get<string>("EmployeeId"));
            ViewBag.Permission = _employeeRepository.GetByObjectId(_employeeId).Role.Name;

            // Load working week for Manager
            var workingWeeks = _workingWeekRepository.LoadSubmittedByManager(_employeeId);

            // Load all working week to View
            var weeks = new WorkingWeekList
            {
                WorkingWeeks = workingWeeks == null ? new List<WorkingWeek>() : workingWeeks
            };

            if (workingWeeks != null)
            {
                // Get all employeeId
                workingWeeks.ForEach(w => employeeId.Add(_employeeId));
                employeeId.Distinct().ToList().ForEach(e => employeeObjects.Add(_employeeRepository.GetByObjectId(e)));

                ViewBag.Employees = employeeObjects; 
            }

            return View(weeks);
        }
    }
}
