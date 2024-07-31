using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.ViewModels;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.Context.Identity;
using Microsoft.AspNetCore.Identity;

namespace OnlineLearningPlatform.Controllers {
    [Authorize(Roles = "Admin")]
    public class QuizQuestionController : BaseController {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public QuizQuestionController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : base(userManager) {
            _db = unitOfWork;
            _mapper = mapper;
        }

        // GET: QuizQuestion
        public IActionResult Index(int quizId) {
            var questions = _db.QuizQuestions.Get(q => q.QuizId == quizId);
            var questionViewModels = _mapper.Map<IEnumerable<QuizQuestionViewModel>>(questions);
            ViewBag.QuizId = quizId;
            return View(questionViewModels);
        }

        // GET: QuizQuestion/Details/5
        public IActionResult Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var question = _db.QuizQuestions.GetByID(id);
            if (question == null) {
                return NotFound();
            }

            var questionViewModel = _mapper.Map<QuizQuestionViewModel>(question);
            return View(questionViewModel);
        }

        // GET: QuizQuestion/Create
        public IActionResult Create(int quizId) {
            ViewBag.QuizId = quizId;
            return View();
        }

        // POST: QuizQuestion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuizQuestionViewModel questionViewModel) {
            if (ModelState.IsValid) {
                var question = _mapper.Map<QuizQuestion>(questionViewModel);
                _db.QuizQuestions.Insert(question);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
            }

            return View(questionViewModel);
        }

        // GET: QuizQuestion/Edit/5
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var question = _db.QuizQuestions.GetByID(id);
            if (question == null) {
                return NotFound();
            }

            var questionViewModel = _mapper.Map<QuizQuestionViewModel>(question);
            return View(questionViewModel);
        }

        // POST: QuizQuestion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, QuizQuestionViewModel questionViewModel) {
            if (id != questionViewModel.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                var question = _mapper.Map<QuizQuestion>(questionViewModel);
                _db.QuizQuestions.Update(question);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
            }

            return View(questionViewModel);
        }

        // GET: QuizQuestion/Delete/5
        public IActionResult Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var question = _db.QuizQuestions.GetByID(id);
            if (question == null) {
                return NotFound();
            }

            var questionViewModel = _mapper.Map<QuizQuestionViewModel>(question);
            return View(questionViewModel);
        }

        // POST: QuizQuestion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) {
            var question = _db.QuizQuestions.GetByID(id);
            _db.QuizQuestions.Delete(question);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
        }
    }
}