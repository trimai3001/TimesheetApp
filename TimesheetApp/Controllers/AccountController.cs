using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Interfaces;
using TimesheetApp.ViewModels;
using TimeSheetApp.Models;

namespace TimesheetApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        [TempData]
        public string Message { get; set; }
        public AccountController(IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                




                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Manage()
        {
            var employeeNotHaveAccount = new List<Employee>();
            var employees = _employeeRepository.LoadAll().ToList();
            var accounts = _userRepository.LoadAll().ToList();

            foreach (var employee in employees)
            {
                var account = accounts.Find(a => a.EmployeeId == employee.Id);
                if (account == null)
                {
                    employeeNotHaveAccount.Add(employee);
                }
            }

            ViewBag.Employees = employeeNotHaveAccount;

            return View();
        }
    }
}
