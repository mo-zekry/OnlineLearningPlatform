using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

public class QuizController : BaseController
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public QuizController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : base(userManager)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var quizzes = _db.Quizzes.Get();
        var quizViewModels = _mapper.Map<IEnumerable<QuizViewModel>>(quizzes);

        // get courses related to each quiz
        this.GetCourses();

        return View(quizViewModels);
    }

    public IActionResult Details(int id)
    {
        var quiz = _db.Quizzes.GetByID(id);
        var quizViewModel = _mapper.Map<QuizViewModel>(quiz);

        // get courses related to each quiz
        this.GetCourses();

        return View(quizViewModel);
    }

    public IActionResult Create()
    {
        // get courses related to each quiz
        this.GetCourses();

        return View(new QuizViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(QuizViewModel quizViewModel)
    {
        if (ModelState.IsValid)
        {
            var quiz = _mapper.Map<Quiz>(quizViewModel);
            _db.Quizzes.Insert(quiz);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("Create", quizViewModel);
    }

    public IActionResult Edit(int id)
    {
        var quiz = _db.Quizzes.GetByID(id);
        var quizViewModel = _mapper.Map<QuizViewModel>(quiz);

        // get courses related to each quiz
        this.GetCourses();

        return View(quizViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(QuizViewModel quizViewModel)
    {
        if (ModelState.IsValid)
        {
            var quiz = _mapper.Map<Quiz>(quizViewModel);
            _db.Quizzes.Update(quiz);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("Edit", quizViewModel);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var quiz = _db.Quizzes.GetByID(id);
        var quizViewModel = _mapper.Map<QuizViewModel>(quiz);

        // get courses related to each quiz
        this.GetCourses();

        return View(quizViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmDelete(int id)
    {
        var quiz = _db.Quizzes.GetByID(id);
        _db.Quizzes.Delete(quiz);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    private void GetCourses()
    {
        var courses = _db.Courses.Get();
        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);
        ViewData["CourseList"] = courseViewModels;
    }
}
