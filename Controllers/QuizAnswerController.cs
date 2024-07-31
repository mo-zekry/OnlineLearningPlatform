using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers {
    [Authorize(Roles = "Admin")]
    public class QuizAnswerController : BaseController {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public QuizAnswerController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : base(userManager) {
            _db = unitOfWork;
            _mapper = mapper;
        }

        // GET: QuizAnswer
        public IActionResult Index(int questionId) {
            var answers = _db.QuizAnswers.Get();
            var answerViewModels = _mapper.Map<IEnumerable<QuizAnswerViewModel>>(answers);
            return View(answerViewModels);
        }

        // GET: QuizAnswer/Details/5
        public IActionResult Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var answer = _db.QuizAnswers.GetByID(id);

            var answerViewModel = _mapper.Map<QuizAnswerViewModel>(answer);
            return View(answerViewModel);
        }

        // GET: QuizAnswer/Create
        public IActionResult Create(int questionId) {
            ViewBag.QuestionId = questionId;
            return View();
        }

        // POST: QuizAnswer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuizAnswerViewModel answerViewModel) {
            if (!ModelState.IsValid) return View(answerViewModel);
            var answer = _mapper.Map<QuizAnswer>(answerViewModel);
            _db.QuizAnswers.Insert(answer);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { questionId = answer.QuestionId });
        }

        // GET: QuizAnswer/Edit/5
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var answer = _db.QuizAnswers.GetByID(id);

            var answerViewModel = _mapper.Map<QuizAnswerViewModel>(answer);
            return View(answerViewModel);
        }

        // POST: QuizAnswer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, QuizAnswerViewModel answerViewModel) {
            if (id != answerViewModel.Id) {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(answerViewModel);
            var answer = _mapper.Map<QuizAnswer>(answerViewModel);
            _db.QuizAnswers.Update(answer);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { questionId = answer.QuestionId });
        }

        // GET: QuizAnswer/Delete/5
        public IActionResult Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var answer = _db.QuizAnswers.GetByID(id);

            var answerViewModel = _mapper.Map<QuizAnswerViewModel>(answer);
            return View(answerViewModel);
        }

        // POST: QuizAnswer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) {
            var answer = _db.QuizAnswers.GetByID(id);
            _db.QuizAnswers.Delete(answer);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { questionId = answer.QuestionId });
        }
    }
}