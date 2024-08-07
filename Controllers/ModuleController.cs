using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

[Authorize(Roles = "Admin")]
public class ModuleController : BaseController
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public ModuleController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager
    )
        : base(userManager)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    // GET: Module
    public IActionResult Index()
    {
        var modules = _db.Modules.Get();
        var moduleViewModels = _mapper.Map<IEnumerable<ModuleViewModel>>(modules);

        // get the courses related to each module
        var listOfCourses = _db.Courses.Get();
        ViewData["CoursesList"] = _mapper.Map<IEnumerable<CourseViewModel>>(listOfCourses);

        return View(moduleViewModels);
    }

    // GET: Module/Create
    public IActionResult Create(int courseId)
    {
        ViewBag.CourseId = courseId;
        // get the list of courses
        var listOfCourses = _db.Courses.Get();
        ViewData["CoursesList"] = _mapper.Map<IEnumerable<CourseViewModel>>(listOfCourses);

        return View(new ModuleViewModel());
    }

    // POST: Module/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ModuleViewModel moduleViewModel)
    {
        if (ModelState.IsValid)
        {
            var module = _mapper.Map<Module>(moduleViewModel);
            _db.Modules.Insert(module);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(moduleViewModel);
    }

    // POST: Module/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, ModuleViewModel moduleViewModel)
    {
        if (id != moduleViewModel.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var module = _mapper.Map<Module>(moduleViewModel);
            _db.Modules.Update(module);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { courseId = module.CourseId });
        }

        return View(moduleViewModel);
    }

    // POST: Module/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var module = _db.Modules.GetByID(id);
        _db.Modules.Delete(module);
        _db.SaveChanges();

        // get related lessons
        foreach (var lesson in _db.Lessons.Get(l => l.ModuleId == id))
        {
            _db.Lessons.Delete(lesson);
        }
        _db.SaveChanges();

        return RedirectToAction(nameof(Index), new { courseId = module.CourseId });
    }
}
