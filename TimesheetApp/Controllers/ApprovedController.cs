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
    public class ApprovedController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkingWeekRepository _workingWeekRepository;

        private ObjectId _employeeId;
        public ApprovedController(IEmployeeRepository employeeRepository, IWorkingWeekRepository workingWeekRepository)
        {
            _employeeRepository = employeeRepository;
            _workingWeekRepository = workingWeekRepository;
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
            var workingWeeks = _workingWeekRepository.LoadApprovedByManager(_employeeId);

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
