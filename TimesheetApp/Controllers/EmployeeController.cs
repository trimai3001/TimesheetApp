using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Interfaces;
using TimesheetApp.ViewModels;
using TimeSheetApp.Models;

namespace TimesheetApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;

        private string employeeIdGenerate;
        public EmployeeController(IEmployeeRepository employeeRepository, IRoleRepository roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            employeeIdGenerate = _employeeRepository.GenerateEmployeeId();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return RedirectToAction(nameof(Manage));
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, IFormCollection form)
        {
            try
            {
                ObjectId roleId = ObjectId.Parse(form["role"].ToString());
                employee.Role = _roleRepository.GetRoleById(roleId);
                employee.EmployeeId = employeeIdGenerate;
                _employeeRepository.CreateEmployee(employee);
                ModelState.Clear();
                ViewBag.ToastStatus = "success";
                ViewBag.ToastMessage = "Create employee successfully";

            }
            catch
            {
                ViewBag.ToastStatus = "error";
                ViewBag.ToastMessage = "Create employee unsuccessfully";
            }
            return RedirectToAction(nameof(Manage));
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete()
        {
            return RedirectToAction(nameof(Manage));
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection form)
        {
            try
            {
                ObjectId id = ObjectId.Parse(form["EmployeeId"].ToString());
                _employeeRepository.DeleteEmployee(id);
                ViewBag.ToastStatus = "success";
                ViewBag.ToastMessage = "Delete employee successfully";
            }
            catch (Exception e)
            {
                ViewBag.ToastStatus = "error";
                ViewBag.ToastMessage = "Delete employee unsuccessfully";
            }
            return RedirectToAction(nameof(Manage));
        }

        public ActionResult Manage()
        {
            ViewBag.Roles = _roleRepository.LoadAll();
            ViewBag.AllEmployee = _employeeRepository.LoadAll();
            var employee = new Employee
            {
                EmployeeId = employeeIdGenerate
            };
            return View(employee);
        }
    }
}
