using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class CategoryController : Controller
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    // Display all categories
    public IActionResult Index()
    {
        var categories = _db.Categories.Get();
        var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

        var courses = _db.Courses.Get();
        var coursesViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);
        ViewData["CourseList"] = coursesViewModels;

        return View(categoryViewModels);
    }

    // Display details of a specific category
    public IActionResult Details(int id)
    {
        var category = _db.Categories.GetByID(id);
        if (category == null)
            return NotFound();

        var categoryViewModel = _mapper.Map<CategoryViewModel>(category);
        return View(categoryViewModel);
    }

    // Create a new category (GET)
    public IActionResult Create()
    {
        return View(new CategoryViewModel());
    }

    // Create a new category (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var category = _mapper.Map<Category>(model);
            _db.Categories.Insert(category);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), model);
        }
        return View(model);
    }

    // Edit an existing category (GET)
    public IActionResult Edit(int id)
    {
        var category = _db.Categories.GetByID(id);
        if (category == null)
            return NotFound();

        var categoryViewModel = _mapper.Map<CategoryViewModel>(category);

        return View(categoryViewModel);
    }

    // Edit an existing category (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var category = _db.Categories.GetByID(model.Id);
            if (category == null)
                return NotFound();
            _mapper.Map(model, category);
            _db.Categories.Update(category);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), model);
        }
        return View(model);
    }

    // Delete a category (GET)
    public IActionResult Delete(int id)
    {
        var category = _db.Categories.GetByID(id);
        if (category == null)
            return NotFound();

        var categoryViewModel = _mapper.Map<CategoryViewModel>(category);
        return View(categoryViewModel);
    }

    // Delete a category (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var category = _db.Categories.GetByID(id);
        if (category == null)
            return NotFound();

        _db.Categories.Delete(category);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
