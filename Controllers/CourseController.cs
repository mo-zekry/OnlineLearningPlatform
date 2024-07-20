using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> Index()
    {
        var courses = await _db.Courses.GetIncludingAsync("Category");
        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

        var categories = await _db.Category.GetAllAsync();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewBag.Categories = categoryViewModels;

        return View(courseViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> Search(string query)
    {
        var results = await _db.Courses.FindAsync(x => x.Name.Contains(query), "Category");

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> GetCourses()
    {
        var results = await _db.Courses.GetIncludingAsync("Category");

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> Filter(int id)
    {
        var results = await _db.Courses.FindAsync(x => x.CategoryId == id, "Category");

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        return PartialView("_CourseListPartial", courseViewModels);
    }

    // course detailes

    public async Task<IActionResult> Details(int id)
    {
        var course = await _db.Courses.GetByIdAsync(id);
        var courseCategory = await _db.Category.GetByIdAsync(course.CategoryId);
        var courseModel = _mapper.Map<CourseViewModel>(course);
        courseModel.CategoryName = courseCategory.Name;
        return View(courseModel);
    }

    // create course

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseViewModel courseViewModel)
    {
        if (ModelState.IsValid)
        {
            var course = _mapper.Map<Course>(courseViewModel);
            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return RedirectToAction("Create");
    }
}
