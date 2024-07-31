using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.ViewModels;

using OnlineLearningPlatform.Repositories;

namespace OnlineLearningPlatform.Controllers {
    [Authorize(Roles = "Student")]
    public class StudentQuizAttemptController : BaseController {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentQuizAttemptController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : base(userManager) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: StudentQuizAttempt
        public IActionResult Index() {
            var userId = _userManager.GetUserId(User);
            var attempts = _unitOfWork.StudentQuizAttempts.Get(sqa => sqa.StudentId == userId);
            var attemptViewModels = _mapper.Map<IEnumerable<StudentQuizAttemptViewModel>>(attempts);
            return View(attemptViewModels);
        }

        // GET: StudentQuizAttempt/Details/5
        public IActionResult Details(int? quizId) {
            if (quizId == null) {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var attempt = _unitOfWork.StudentQuizAttempts.Get(sqa => sqa.QuizId == quizId && sqa.StudentId == userId)
                .FirstOrDefault();

            if (attempt == null) {
                return NotFound();
            }

            var attemptViewModel = _mapper.Map<StudentQuizAttemptViewModel>(attempt);
            return View(attemptViewModel);
        }

        // POST: StudentQuizAttempt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentQuizAttemptViewModel attemptViewModel) {
            if (ModelState.IsValid) {
                var attempt = _mapper.Map<StudentQuizAttempt>(attemptViewModel);
                attempt.StudentId = _userManager.GetUserId(User);
                _unitOfWork.StudentQuizAttempts.Insert(attempt);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(attemptViewModel);
        }

        // POST: StudentQuizAttempt/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Complete(int quizId) {
            var userId = _userManager.GetUserId(User);
            var attempt = _unitOfWork.StudentQuizAttempts.Get(sqa => sqa.QuizId == quizId && sqa.StudentId == userId)
                .FirstOrDefault();

            if (attempt == null) {
                return NotFound();
            }

            attempt.AttemptDatetime = DateTime.UtcNow;
            _unitOfWork.StudentQuizAttempts.Update(attempt);
            _unitOfWork.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}