using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class CourseController : Controller
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public CourseController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var courses = _db.Courses.Get(includeProperties: "Category");

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return View(courseViewModels);
    }

    [HttpPost]
    public IActionResult Search(string query)
    {
        var results = _db.Courses.Get(filter: x => x.Name.Contains(query));

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public IActionResult GetCourses()
    {
        var results = _db.Courses.Get();

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public IActionResult Filter(int id)
    {
        var results = _db.Courses.Get(filter: x => x.CategoryId == id);

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewData["Categories"] = categoryViewModels;

        return PartialView("_CourseListPartial", courseViewModels);
    }

    // course detailes

    public IActionResult Details(int id)
    {
        var course = _db.Courses.GetByID(id);
        var courseModel = _mapper.Map<CourseViewModel>(course);

        var courseCategory = _db.Categories.GetByID(course.CategoryId);
        ViewBag.CategoryName = courseCategory.Name;

        var modules = _db.Modules.Get(filter: x => x.CourseId == id);
        var moduleViewModels = _mapper.Map<IEnumerable<ModuleViewModel>>(modules);
        ViewData["Modules"] = moduleViewModels;

        var lessons = _db.Lessons.Get(filter: x => x.ModuleId == id);
        var lessonViewModels = _mapper.Map<IEnumerable<LessonViewModel>>(lessons);
        ViewData["Lessons"] = lessonViewModels;
        
        return View(courseModel);
    }

    // create course

    public IActionResult Create()
    {
        var categories = _db.Categories.Get();
        ViewBag.Categories = categories;

        return View(new CourseViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseViewModel courseViewModel)
    {
        if (ModelState.IsValid)
        {
            var category = _db.Categories.GetByID(courseViewModel.CategoryId);
            var course = _mapper.Map<Course>(courseViewModel);
            course.Category = category;
            _db.Courses.Insert(course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View("Create", courseViewModel);
    }

    // edit course
}
