using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class HomeController : Controller {
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public HomeController(IUnitOfWork unitOfWork, IMapper mapper) {
        _db = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index() {
        var courses = await _db.Courses.GetIncludingAsync("Category");
        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

        var categories = await _db.Category.GetAllAsync();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        ViewBag.Categories = categoryViewModels;

        return View(courseViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> Search(string query) {
        var results = await _db.Courses.FindAsync(x => x.Name.Contains(query), "Category");

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> GetCourses() {
        var results = await _db.Courses.GetIncludingAsync("Category");

        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(results);

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> Filter(int id) {
        var results = await _db.Courses.FindAsync(x => x.CategoryId == id, "Category");

        var courseViewModels =  _mapper.Map<IEnumerable<CourseViewModel>>(results);

        return PartialView("_CourseListPartial", courseViewModels);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}