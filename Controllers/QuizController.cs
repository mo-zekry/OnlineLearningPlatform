using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers
{
    [Authorize(Roles = "Admin, Student")]
    public class QuizController : BaseController
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
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

            // Retrieve courses and map them to the ViewModel
            ViewData["CoursesViewModel"] = _mapper.Map<IEnumerable<CourseViewModel>>(
                _db.Courses.Get()
            );
            ViewData["QuizViewModel"] = new QuizViewModel();

            return View(quizViewModels);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var quiz = _db.Quizzes.GetByID(id);
            if (quiz == null)
                return NotFound();

            // Authorization check
            var authorizationResult = _authorizationService
                .AuthorizeAsync(User, quiz.CourseId, "EnrolledInCourse")
                .Result;
            if (!authorizationResult.Succeeded && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Create", "Enrollment", new { courseId = quiz.CourseId });
            }

            var quizViewModel = _mapper.Map<QuizViewModel>(quiz);

            // Get and shuffle quiz questions and answers
            var questions = _db
                .QuizQuestions.Get(q => q.QuizId == id)
                .OrderBy(q => Guid.NewGuid())
                .ToList();
            var answerViewModels = questions
                .SelectMany(q =>
                    _mapper.Map<IEnumerable<QuizAnswerViewModel>>(
                        _db.QuizAnswers.Get(a => a.QuestionId == q.Id).OrderBy(a => Guid.NewGuid())
                    )
                )
                .ToList();

            ViewData["Questions"] = _mapper.Map<IEnumerable<QuizQuestionViewModel>>(questions);
            ViewData["Answers"] = answerViewModels;

            return View(quizViewModel);
        }

        [HttpPost]
        public IActionResult Details(int id, Dictionary<int, int> Answers)
        {
            var quiz = _db.Quizzes.GetByID(id);
            if (quiz == null)
                return NotFound();

            var questions = _db.QuizQuestions.Get(q => q.QuizId == id).ToList();
            var correctAnswers = questions.Count(q =>
                Answers.TryGetValue(q.Id, out int selectedAnswerId)
                && _db.QuizAnswers.Get(a => a.QuestionId == q.Id && a.IsCorrect)
                    .FirstOrDefault()
                    ?.Id == selectedAnswerId
            );

            var percentage = (double)correctAnswers / questions.Count * 100;
            var isPassed = percentage >= quiz.MinPassScore;

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Error", new { message = "User does not exist." });

            var attempt = new StudentQuizAttempt
            {
                StudentId = userId,
                QuizId = id,
                AttemptDatetime = DateTime.Now,
                ScoreAchieved = (int)percentage,
            };

            _db.StudentQuizAttempts.Insert(attempt);
            _db.SaveChanges();

            return RedirectToAction(isPassed ? "Passed" : "Failed", new { id = attempt.QuizId });
        }

        public IActionResult Passed(int id)
        {
            var result = _db
                .StudentQuizAttempts.Get(st =>
                    st.QuizId == id && st.StudentId == _userManager.GetUserId(User)
                )
                .FirstOrDefault();

            return View(result);
        }

        public IActionResult Failed(int id)
        {
            var result = _db
                .StudentQuizAttempts.Get(st =>
                    st.QuizId == id && st.StudentId == _userManager.GetUserId(User)
                )
                .FirstOrDefault();

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
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var quiz = _db.Quizzes.GetByID(id);
            if (quiz == null)
                return NotFound();

            var quizViewModel = _mapper.Map<QuizViewModel>(quiz);
            GetCourses();

            return View(quizViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, QuizViewModel quizViewModel)
        {
            if (id != quizViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var quiz = _mapper.Map<Quiz>(quizViewModel);
                quiz.Course = _db.Courses.GetByID(quizViewModel.CourseId);
                _db.Quizzes.Update(quiz);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            GetCourses();
            return View(quizViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var quiz = _db.Quizzes.GetByID(id);
            if (quiz == null)
                return NotFound();

            var quizViewModel = _mapper.Map<QuizViewModel>(quiz);
            GetCourses();

            return View(quizViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            var quiz = _db.Quizzes.GetByID(id);
            if (quiz == null)
                return NotFound();

            var questions = _db.QuizQuestions.Get(q => q.QuizId == id).ToList();
            var answers = questions
                .SelectMany(q => _db.QuizAnswers.Get(a => a.QuestionId == q.Id))
                .ToList();

            _db.QuizAnswers.DeleteRange(answers);
            _db.QuizQuestions.DeleteRange(questions);
            _db.Quizzes.Delete(quiz);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void GetCourses()
        {
            ViewData["CourseList"] = _mapper.Map<IEnumerable<CourseViewModel>>(_db.Courses.Get());
        }
    }
}
