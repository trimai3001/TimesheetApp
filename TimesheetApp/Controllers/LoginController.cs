using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TimesheetApp.Helper;
using TimesheetApp.Interfaces;
using TimeSheetApp.Models;

namespace TimesheetApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private IEnumerable<User> _user;
        public LoginController(IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository;
            _user = _userRepository.LoadAll();
            _employeeRepository = employeeRepository;
            ViewBag.Message = "";
        }

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var member = _user.ToList().Find(u => u.Username.ToString().ToLower() == user.Username.ToLower() && u.Password == Utilities.GetMD5(user.Password));

            if(member == null)
            {
                ViewBag.Message = "Your username or password is incorrect. Please try again.";
                return View();
            }

            ViewBag.Message = "";
            
            //var employeeInfo = _employeeRepository.GetByObjectId(member.EmployeeId);
            HttpContext.Session.Set("EmployeeId", member.EmployeeId.ToString());

            return RedirectToAction("Manage", "Home");
        }
    }
}
