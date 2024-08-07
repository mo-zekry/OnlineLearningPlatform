using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

[Authorize(Roles = "Admin, Student")]
public class QuizController : BaseController
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;
    IAuthorizationService _authorizationService;

    private readonly UserManager<ApplicationUser> _userManager;

    public QuizController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IAuthorizationService authorizationService
    )
        : base(userManager)
    {
        _db = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    public IActionResult Index()
    {
        var quizzes = _db.Quizzes.Get();
        var quizViewModels = _mapper.Map<IEnumerable<QuizViewModel>>(quizzes);

        // get courses
        var courses = _db.Courses.Get();
        ViewData["CoursesViewModel"] = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

        ViewData["QuizViewModel"] = new QuizViewModel();

        return View(quizViewModels);
    }

    // GET: Quiz/Details/5
    [HttpGet]
 public IActionResult Details(int id)
{
    var quiz = _db.Quizzes.GetByID(id);
    var quizViewModel = _mapper.Map<QuizViewModel>(quiz);

    // Check if user is authorized
    var authorizationResult = _authorizationService
        .AuthorizeAsync(User, quiz.CourseId, "EnrolledInCourse")
        .Result;
    if (!authorizationResult.Succeeded && !User.IsInRole("Admin"))
    {
        return RedirectToAction("Create", "Enrollment", new { courseId = quiz.CourseId });
    }

    // Get questions related to this quiz
    var questions = _db.QuizQuestions.Get(q => q.QuizId == id).ToList();
    var shuffledQuestions = questions.OrderBy(q => Guid.NewGuid()).ToList();
    ViewData["Questions"] = _mapper.Map<IEnumerable<QuizQuestionViewModel>>(shuffledQuestions);

    // Get the answers related to this quiz
    var answerViewModels = new List<QuizAnswerViewModel>();
    foreach (var question in shuffledQuestions)
    {
        var answers = _db.QuizAnswers.Get(a => a.QuestionId == question.Id).ToList();
        var shuffledAnswers = answers.OrderBy(a => Guid.NewGuid()).ToList();
        answerViewModels.AddRange(_mapper.Map<IEnumerable<QuizAnswerViewModel>>(shuffledAnswers));
    }
    ViewData["Answers"] = answerViewModels;

    return View(quizViewModel);
}


    // POST: Quiz/Details/5
    [HttpPost]
    public IActionResult Details(int id, Dictionary<int, int> Answers)
    {
        var quiz = _db.Quizzes.GetByID(id);
        var questions = _db.QuizQuestions.Get(q => q.QuizId == id);

        // Calculate score
        int correctAnswers = 0;
        foreach (var question in questions)
        {
            if (Answers.TryGetValue(question.Id, out int selectedAnswerId))
            {
                var correctAnswer = _db
                    .QuizAnswers.Get(a => a.QuestionId == question.Id && a.IsCorrect)
                    .FirstOrDefault();
                if (correctAnswer != null && correctAnswer.Id == selectedAnswerId)
                {
                    correctAnswers++;
                }
            }
        }

        // Convert score to percentage
        double percentage = (double)correctAnswers / questions.Count() * 100;

        // Determine if the student passed
        bool isPassed = percentage >= quiz.MinPassScore;

        // Get user ID
        var userId = _userManager.GetUserId(User);

        // Ensure user exists
        var userExists = _userManager.Users.Any(u => u.Id == userId);
        if (!userExists)
        {
            return RedirectToAction("Error", new { message = "User does not exist." });
        }

        // Store the student's attempt
        var attempt = new StudentQuizAttempt
        {
            StudentId = userId ?? string.Empty,
            QuizId = id,
            AttemptDatetime = DateTime.Now,
            ScoreAchieved = (int)percentage,
        };

        // Save the attempt in the database
        _db.StudentQuizAttempts.Insert(attempt);
        _db.SaveChanges();

        // Redirect based on pass/fail
        if (isPassed)
        {
            return RedirectToAction("Passed", new { id = attempt.QuizId });
        }
        else
        {
            return RedirectToAction("Failed", new { id = attempt.QuizId });
        }
    }

    public IActionResult Passed(int id)
    {
        var result = _db.StudentQuizAttempts.Get(st => st.QuizId == id && st.StudentId == _userManager.GetUserId(User)).ElementAt(0);
        return View(result);
    }

    public IActionResult Failed(int id)
    {
        var result = _db.StudentQuizAttempts.Get(st => st.QuizId == id && st.StudentId == _userManager.GetUserId(User)).ElementAt(0);
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateQuiz(QuizViewModel model)
    {
        if (ModelState.IsValid)
        {
            var quiz = _mapper.Map<Quiz>(model);
            _db.Quizzes.Insert(quiz);
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // GET: Quiz/Edit/5
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var quiz = _db.Quizzes.GetByID(id);
        if (quiz == null)
            return NotFound();

        var quizViewModel = _mapper.Map<QuizViewModel>(quiz);

        // get courses related to each quiz
        GetCourses();

        return View(quizViewModel);
    }

    // POST: Course/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, QuizViewModel quizViewModel)
    {
        if (id != quizViewModel.Id)
            return NotFound();
        var course = _db.Courses.GetByID(quizViewModel.CourseId);

        if (ModelState.IsValid)
        {
            var quiz = _mapper.Map<Quiz>(quizViewModel);
            // get course by id
            quiz.Course = course;
            _db.Quizzes.Update(quiz);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(quizViewModel);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var quiz = _db.Quizzes.GetByID(id);
        var quizViewModel = _mapper.Map<QuizViewModel>(quiz);

        // get courses related to each quiz
        GetCourses();

        return View(quizViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmDelete(int id)
    {
        var quiz = _db.Quizzes.GetByID(id);
        _db.Quizzes.Delete(quiz);
        _db.SaveChanges();

        // get related questions
        foreach (var item in _db.QuizQuestions.Get(q => q.QuizId == id))
        {
            _db.QuizQuestions.Delete(item);
            _db.SaveChanges();

            // get related answers
            foreach (var answer in _db.QuizAnswers.Get(a => a.QuestionId == item.Id))
            {
                _db.QuizAnswers.Delete(answer);
                _db.SaveChanges();
            }
        }

        return RedirectToAction("Index");
    }


    private void GetCourses()
    {
        var courses = _db.Courses.Get();
        ViewData["CourseList"] = _mapper.Map<IEnumerable<CourseViewModel>>(courses);
    }
}
