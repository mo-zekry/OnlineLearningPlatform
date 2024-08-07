using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers;

[Authorize(Roles = "Admin")]
public class QuizQuestionController : BaseController
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public QuizQuestionController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager
    )
        : base(userManager)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    // GET: QuizQuestion
    public IActionResult Index(int quizId)
    {
        var questions = _db.QuizQuestions.Get(q => q.QuizId == quizId);
        var questionViewModels = _mapper.Map<IEnumerable<QuizQuestionViewModel>>(questions);
        ViewBag.QuizId = quizId;

        // get the answer related this quiz
        List<QuizAnswer> answers = [];
        foreach (var item in questions)
        {
            answers.AddRange(_db.QuizAnswers.Get(a => a.QuestionId == item.Id));
        }

        ViewBag.Answers = answers;

        return View(questionViewModels);
    }

    // GET: QuizQuestion/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
            return NotFound();

        var question = _db.QuizQuestions.GetByID(id);
        if (question == null)
            return NotFound();

        // get answer related to each question
        var answers = _db.QuizAnswers.Get(a => a.QuestionId == id);
        ViewData["Answers"] = _mapper.Map<IEnumerable<QuizAnswerViewModel>>(answers);

        // get the question related this quiz
        var quizzes = _db.Quizzes.Get();
        ViewData["Quizzes"] = _mapper.Map<IEnumerable<QuizViewModel>>(quizzes);

        var questionViewModel = _mapper.Map<QuizQuestionViewModel>(question);
        return View(questionViewModel);
    }

    // GET: QuizQuestion/Create
    public IActionResult Create(int quizId)
    {
        ViewBag.QuizId = quizId;
        return View(new QuizQuestionViewModel());
    }

    // POST: QuizQuestion/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(QuizQuestionViewModel questionViewModel)
    {
        if (ModelState.IsValid)
        {
            var question = _mapper.Map<QuizQuestion>(questionViewModel);
            _db.QuizQuestions.Insert(question);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
        }

        return View(questionViewModel);
    }

    // GET: QuizQuestion/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var question = _db.QuizQuestions.GetByID(id);
        if (question == null)
            return NotFound();

        var questionViewModel = _mapper.Map<QuizQuestionViewModel>(question);
        return View(questionViewModel);
    }

    // POST: QuizQuestion/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, QuizQuestionViewModel questionViewModel)
    {
        if (id != questionViewModel.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var question = _mapper.Map<QuizQuestion>(questionViewModel);
            _db.QuizQuestions.Update(question);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
        }

        return View(questionViewModel);
    }

    // POST: QuizQuestion/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteQuestion")]
    public IActionResult Delete(int id)
    {
        var question = _db.QuizQuestions.GetByID(id);
        _db.QuizQuestions.Delete(question);
        _db.SaveChanges();

        // get answers related to question
        foreach (var item in _db.QuizAnswers.Get(a => a.QuestionId == id))
        {
            _db.QuizAnswers.Delete(item);
        }
        _db.SaveChanges();

        return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
    }
}
