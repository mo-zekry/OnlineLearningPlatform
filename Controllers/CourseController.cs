using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

[Authorize(Roles = "Admin,Student")]
public class CourseController : BaseController
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;

    public CourseController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IAuthorizationService authorizationService
    )
        : base(userManager)
    {
        _db = unitOfWork;
        _mapper = mapper;
        _authorizationService = authorizationService;
    }

    // GET: Course
    [AllowAnonymous]
    public IActionResult Index()
    {
        var courses = _db.Courses.Get();
        var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

        var categories = _db.Categories.Get();
        ViewData["Categories"] = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

        return View(courseViewModels);
    }

    // GET: Course
    [AllowAnonymous]
    public async Task<IActionResult> DetailsAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var course = _db.Courses.GetByID(id);
        if (course == null)
            return NotFound();

        // Pass the course ID as the resource when checking the policy
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            id,
            "EnrolledInCourse"
        );
        if (!authorizationResult.Succeeded && !User.IsInRole("Admin"))
        {
            return RedirectToAction("Create", "Enrollment", new { courseId = id });
        }

        var modules = _db.Modules.Get(m => m.CourseId == id);
        ViewData["Modules"] = modules;

        foreach (var module in modules)
        {
            var moduleLessons = _db.Lessons.Get(l => l.ModuleId == module.Id);
            module.Lessons = new List<Lesson>(moduleLessons);
        }

        var lessons = _db.Lessons.Get(l => l.ModuleId == id);

        var quizzes = _db.Quizzes.Get(q => q.CourseId == id);
        ViewData["Quizzes"] = quizzes;

        // get questions related to each quiz
        var quizQuestions = new List<QuizQuestion>();
        foreach (var quiz in quizzes)
        {
            var quizQuestionsForQuiz = _db.QuizQuestions.Get(q => q.QuizId == quiz.Id);
            quizQuestions.AddRange(quizQuestionsForQuiz);
            quiz.QuizQuestions = new List<QuizQuestion>(quizQuestionsForQuiz);
        }

        // get answers related to each question
        foreach (var quizQuestion in quizQuestions)
        {
            var quizAnswers = _db.QuizAnswers.Get(q => q.Id == quizQuestion.Id);
            quizQuestion.QuizAnswers = new List<QuizAnswer>(quizAnswers);
        }

        var courseViewModel = _mapper.Map<CourseViewModel>(course);
        ViewData["CourseViewModel"] = courseViewModel;

        return View(courseViewModel);
    }

    // GET: Course/Create
    public IActionResult Create()
    {
        // get categories
        var categories = _db.Categories.Get();
        ViewData["CategoriesList"] = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        return View();
    }

    // POST: Course/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(CourseViewModel courseViewModel)
    {
        if (ModelState.IsValid)
        {
            var category = _db.Categories.GetByID(courseViewModel.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Invalid Category.");
                return View(courseViewModel);
            }

            var course = _mapper.Map<Course>(courseViewModel);
            course.Category = category;

            _db.Courses.Insert(course);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(courseViewModel);
    }

    // GET: Course/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var course = _db.Courses.GetByID(id);
        if (course == null)
            return NotFound();

        var categories = _db.Categories.Get();
        ViewData["CategoriesList"] = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

        var courseViewModel = _mapper.Map<CourseViewModel>(course);
        return View(courseViewModel);
    }

    // POST: Course/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, CourseViewModel courseViewModel)
    {
        if (id != courseViewModel.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var course = _mapper.Map<Course>(courseViewModel);

            var category = _db.Categories.GetByID(courseViewModel.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Invalid Category.");
                return View(courseViewModel);
            }
            course.Category = category;

            _db.Courses.Update(course);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(courseViewModel);
    }

    // GET: Course/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var course = _db.Courses.GetByID(id);
        if (course == null)
            return NotFound();

        var courseViewModel = _mapper.Map<CourseViewModel>(course);
        return View(courseViewModel);
    }

    // POST: Course/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        // delete course
        var course = _db.Courses.GetByID(id);
        _db.Courses.Delete(course);
        _db.SaveChanges();

        // get related modules
        foreach (var module in _db.Modules.Get(m => m.CourseId == id))
        {
            _db.Modules.Delete(module);
        }

        _db.SaveChanges();

        // get related lessons
        foreach (var lesson in _db.Lessons.Get(l => l.ModuleId == id))
        {
            _db.Lessons.Delete(lesson);
        }

        _db.SaveChanges();

        // get related quizzes
        foreach (var quiz in _db.Quizzes.Get(q => q.CourseId == id))
        {
            _db.Quizzes.Delete(quiz);
        }

        _db.SaveChanges();

        // get related questions
        foreach (var question in _db.QuizQuestions.Get(q => q.QuizId == id))
        {
            _db.QuizQuestions.Delete(question);
        }

        _db.SaveChanges();

        // get related answers
        foreach (var answer in _db.QuizAnswers.Get(a => a.QuestionId == id))
        {
            _db.QuizAnswers.Delete(answer);
        }

        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
