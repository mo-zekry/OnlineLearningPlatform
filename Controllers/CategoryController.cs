using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class CategoryController : Controller
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public CategoryController(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var categories = _db.Categories.Get();
        var categoriesViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

        var courses = _db.Courses.Get();
        var coursesViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);
        ViewData["CourseList"] = coursesViewModels;

        return View(categoriesViewModels);
    }
}
