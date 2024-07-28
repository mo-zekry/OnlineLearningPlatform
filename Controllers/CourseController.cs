using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;

using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public CourseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _db = unitOfWork;
            _mapper = mapper;
        }

        // GET: Course
        [AllowAnonymous]
        public IActionResult Index()
        {
            var courses = _db.Courses.Get();
            var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

            var categories = _db.Categories.Get();
            var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            ViewData["Categories"] = categoryViewModels;

            return View(courseViewModels);
        }

        // GET: Course/Details/5
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _db.Courses.GetByID(id);
            if (course == null)
            {
                return NotFound();
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);
            return View(courseViewModel);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
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
                var course = _mapper.Map<Course>(courseViewModel);
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
            {
                return NotFound();
            }

            var course = _db.Courses.GetByID(id);
            if (course == null)
            {
                return NotFound();
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);
            return View(courseViewModel);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CourseViewModel courseViewModel)
        {
            if (id != courseViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(courseViewModel);
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
            {
                return NotFound();
            }

            var course = _db.Courses.GetByID(id);
            if (course == null)
            {
                return NotFound();
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);
            return View(courseViewModel);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _db.Courses.GetByID(id);
            _db.Courses.Delete(course);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
