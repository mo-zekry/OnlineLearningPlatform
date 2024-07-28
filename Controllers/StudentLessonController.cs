using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.ViewModels;
using OnlineLearningPlatform.Repositories;

namespace OnlineLearningPlatform.Controllers {
    [Authorize(Roles = "Student")]
    public class StudentLessonController : Controller {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentLessonController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager) {
            _db = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: StudentLesson
        public IActionResult Index() {
            var userId = _userManager.GetUserId(User);
            var studentLessons = _db.StudentLessons.Get(sl => sl.StudentId == userId);
            var studentLessonViewModels = _mapper.Map<IEnumerable<StudentLessonViewModel>>(studentLessons);
            return View(studentLessonViewModels);
        }

        // GET: StudentLesson/Details/5
        public IActionResult Details(int? lessonId) {
            if (lessonId == null) {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var studentLesson = _db.StudentLessons.Get(sl => sl.LessonId == lessonId && sl.StudentId == userId)
                .FirstOrDefault();

            if (studentLesson == null) {
                return NotFound();
            }

            var studentLessonViewModel = _mapper.Map<StudentLessonViewModel>(studentLesson);
            return View(studentLessonViewModel);
        }

        // POST: StudentLesson/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Complete(int lessonId) {
            var userId = _userManager.GetUserId(User);
            var studentLesson = _db.StudentLessons.Get(sl => sl.LessonId == lessonId && sl.StudentId == userId)
                .FirstOrDefault();

            if (studentLesson == null) {
                return NotFound();
            }

            studentLesson.CompletedDatetime = DateTime.UtcNow;
            _db.StudentLessons.Update(studentLesson);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}