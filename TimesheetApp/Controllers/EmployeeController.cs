using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimesheetApp.ViewModels;
using TimeSheetApp.Models;

namespace TimesheetApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;
        private Employee employee;

        private string employeeIdGenerate;
        public EmployeeController(IEmployeeRepository employeeRepository, IRoleRepository roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            employeeIdGenerate = _employeeRepository.GenerateEmployeeId();
            
            employee = new Employee
            {
                EmployeeId = employeeIdGenerate
            };

            
        }
        
        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, IFormCollection form)
        {
            try
            {
                ObjectId roleId = ObjectId.Parse(form["Role"].ToString());
                employee.Role = _roleRepository.GetRoleById(roleId);
                employee.EmployeeId = employeeIdGenerate;
                _employeeRepository.CreateEmployee(employee);
                ModelState.Clear();
            }
            catch
            {
            }
            return RedirectToAction(nameof(Manage));
        }

        public ActionResult Delete()
        {
            ViewBag.AllEmployee = _employeeRepository.LoadAll();
            ViewBag.Roles = _roleRepository.LoadAll();
            return View();
        }
        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection form)
        {
            try
            {
                ObjectId id = ObjectId.Parse(form["Id"].ToString());
                _employeeRepository.DeleteEmployee(id);
            }
            catch (Exception e)
            {
            }
            return RedirectToAction(nameof(Manage));
        }

        public ActionResult Manage()
        {
            ViewBag.Test = HttpContext.Session.Get<Employee>("Employee");
            ViewBag.Roles = _roleRepository.LoadAll();
            ViewBag.AllEmployee = _employeeRepository.LoadAll();            
            return View(employee);
        }
    }
}
