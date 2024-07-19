using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
}
