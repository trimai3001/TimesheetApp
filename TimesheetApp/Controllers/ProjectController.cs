using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Interfaces;
using TimeSheetApp.Models;

namespace TimesheetApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private Project _project;
        public ProjectController(IProjectRepository projectRepositor, IEmployeeRepository employeeRepository)
        {
            _projectRepository = projectRepositor;
            _employeeRepository = employeeRepository;
            _project = new Project();
        }

        public ActionResult Manage()
        {
            ViewBag.AllProject = _projectRepository.LoadAll();
            ViewBag.Manager = _employeeRepository.GetAllByRole("Manager");
            return View(_project);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project, IFormCollection form)
        {
            try
            {
                ObjectId id = ObjectId.Parse(form["Manager"].ToString());
                project.Manager = _employeeRepository.GetByObjectId(id);
                _projectRepository.Create(project);
                ModelState.Clear();
            }
            catch(Exception e)
            {
            }
            return RedirectToAction(nameof(Manage));
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection form)
        {
            try
            {
                ObjectId id = ObjectId.Parse(form["id"].ToString());
                _projectRepository.Delete(id);
            }
            catch (Exception e)
            {
            }
            return RedirectToAction(nameof(Manage));
        }
    }
}
