using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class ModuleController : Controller
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public ModuleController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    // Display all modules for a specific course
    public IActionResult Index(int courseId)
    {
        var modules = _db.Modules.Get(filter: x => x.CourseId == courseId);
        var moduleViewModels = _mapper.Map<IEnumerable<ModuleViewModel>>(modules);

        var course = _db.Courses.GetByID(courseId);
        var courseViewModel = _mapper.Map<CourseViewModel>(course);
        ViewData["ModulCourse"] = courseViewModel;

        return View(moduleViewModels);
    }

    // Create a new module (GET)
    // pass [Moudule/Create?courseId=1]
    public IActionResult Create(int courseId)
    {
        var course = _db.Courses.GetByID(courseId);
        if (course == null)
            return NotFound();

        var courseViewModel = _mapper.Map<CourseViewModel>(course);
        ViewData["CourseList"] = courseViewModel;
        var moduleViewModel = new ModuleViewModel() { CourseId = courseId };

        // get modeles Numbers
        var modules = _db.Modules.Get(filter: x => x.CourseId == courseId);
        moduleViewModel.Number = modules.Count() + 1;

        return View(moduleViewModel);
    }

    // Create a new module (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ModuleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var module = _mapper.Map<Module>(model);
            _db.Modules.Insert(module);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { courseId = model.CourseId });
        }
        return View(model);
    }

    // Edit an existing module (GET)
    public IActionResult Edit(int id)
    {
        var module = _db.Modules.GetByID(id);
        if (module == null)
            return NotFound();

        var moduleViewModel = _mapper.Map<ModuleViewModel>(module);

        return View(moduleViewModel);
    }

    // Edit an existing module (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ModuleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var module = _db.Modules.GetByID(model.Id);
            if (module == null)
                return NotFound();

            _mapper.Map(model, module);
            _db.Modules.Update(module);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { courseId = model.CourseId });
        }
        return View(model);
    }

    // Delete a module (GET)
    public IActionResult Delete(int id)
    {
        var module = _db.Modules.GetByID(id);
        if (module == null)
            return NotFound();

        var moduleViewModel = _mapper.Map<ModuleViewModel>(module);
        return View(moduleViewModel);
    }

    // Delete a module (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var module = _db.Modules.GetByID(id);
        if (module == null)
            return NotFound();

        _db.Modules.Delete(module);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index), new { courseId = module.CourseId });
    }
}
