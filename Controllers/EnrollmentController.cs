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
    [Authorize(Roles = "Admin,Student")]
    public class EnrollmentController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnrollmentController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager
        )
        {
            _db = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Enrollment
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var enrollments = _db.Enrollments.Get(e => e.StudentId == userId);
            var enrollmentViewModels = _mapper.Map<IEnumerable<EnrollmentViewModel>>(enrollments);

            var courses = _db.Courses.Get();
            ViewData["CoursesList"] = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

            return View(enrollmentViewModels);
        }

        // GET: Enrollment/Create
        [Authorize(Roles = "Student")]
        public IActionResult Create(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            var enrollmentExists = _db
                .Enrollments.Get(e => e.CourseId == courseId && e.StudentId == userId)
                .Any();

            if (enrollmentExists)
            {
                return RedirectToAction(nameof(Index));
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = userId,
                EnrollmentDatetime = DateTime.UtcNow
            };

            _db.Enrollments.Insert(enrollment);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Enrollment/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int courseId, string studentId)
        {
            var enrollment = _db
                .Enrollments.Get(e => e.CourseId == courseId && e.StudentId == studentId)
                .FirstOrDefault();

            if (enrollment == null)
            {
                return NotFound();
            }

            var enrollmentViewModel = _mapper.Map<EnrollmentViewModel>(enrollment);
            return View(enrollmentViewModel);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int courseId, string studentId)
        {
            var enrollment = _db
                .Enrollments.Get(e => e.CourseId == courseId && e.StudentId == studentId)
                .FirstOrDefault();
            _db.Enrollments.Delete(enrollment);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
