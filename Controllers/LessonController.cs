using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

// [Authorize(Roles = "Admin")]
public class LessonController : BaseController
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public LessonController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager
    )
        : base(userManager)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    // GET: Lesson
    public IActionResult Index(int moduleId)
    {
        var lessons = _db.Lessons.Get(filter: l => l.ModuleId == moduleId);

        var lessonViewModels = _mapper.Map<IEnumerable<LessonViewModel>>(lessons);

        ViewBag.ModuleId = moduleId;

        // get related module
        var module = _db.Modules.Get();
        ViewData["Module"] = _mapper.Map<IEnumerable<ModuleViewModel>>(module);

        return View(lessonViewModels);
    }

    // GET: Lesson/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
            return NotFound();

        var lesson = _db.Lessons.GetByID(id);

        var lessonViewModel = _mapper.Map<LessonViewModel>(lesson);
        return View(lessonViewModel);
    }

    // GET: Lesson/Create
    public IActionResult Create(int moduleId)
    {
        ViewBag.ModuleId = moduleId;

        return View(new LessonViewModel());
    }

    // POST: Lesson/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(LessonViewModel lessonViewModel)
    {
        if (!ModelState.IsValid)
            return View(lessonViewModel);
        var lesson = _mapper.Map<Lesson>(lessonViewModel);
        _db.Lessons.Insert(lesson);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index), new { moduleId = lesson.ModuleId });
    }

    // GET: Lesson/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var lesson = _db.Lessons.GetByID(id);

        var lessonViewModel = _mapper.Map<LessonViewModel>(lesson);
        return View(lessonViewModel);
    }

    // POST: Lesson/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, LessonViewModel lessonViewModel)
    {
        if (id != lessonViewModel.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(lessonViewModel);
        var lesson = _mapper.Map<Lesson>(lessonViewModel);
        _db.Lessons.Update(lesson);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index), new { moduleId = lesson.ModuleId });
    }

    // GET: Lesson/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var lesson = _db.Lessons.GetByID(id);
        var lessonViewModel = _mapper.Map<LessonViewModel>(lesson);
        return View(lessonViewModel);
    }

    // POST: Lesson/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var lesson = _db.Lessons.GetByID(id);
        _db.Lessons.Delete(lesson);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index), new { moduleId = lesson.ModuleId });
    }
}
