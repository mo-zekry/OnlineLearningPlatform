using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public ModuleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _db = unitOfWork;
            _mapper = mapper;
        }

        // GET: Module
        public IActionResult Index(int courseId)
        {
            var modules = _db.Modules.Get(m => m.CourseId == courseId);
            var moduleViewModels = _mapper.Map<IEnumerable<ModuleViewModel>>(modules);
            ViewBag.CourseId = courseId;
            return View(moduleViewModels);
        }

        // GET: Module/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = _db.Modules.GetByID(id);
            if (module == null)
            {
                return NotFound();
            }

            var moduleViewModel = _mapper.Map<ModuleViewModel>(module);
            return View(moduleViewModel);
        }

        // GET: Module/Create
        public IActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: Module/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ModuleViewModel moduleViewModel)
        {
            if (ModelState.IsValid)
            {
                var module = _mapper.Map<Module>(moduleViewModel);
                _db.Modules.Insert(module);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { courseId = module.CourseId });
            }
            return View(moduleViewModel);
        }

        // GET: Module/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = _db.Modules.GetByID(id);
            if (module == null)
            {
                return NotFound();
            }

            var moduleViewModel = _mapper.Map<ModuleViewModel>(module);
            return View(moduleViewModel);
        }

        // POST: Module/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ModuleViewModel moduleViewModel)
        {
            if (id != moduleViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var module = _mapper.Map<Module>(moduleViewModel);
                _db.Modules.Update(module);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { courseId = module.CourseId });
            }
            return View(moduleViewModel);
        }

        // GET: Module/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = _db.Modules.GetByID(id);
            if (module == null)
            {
                return NotFound();
            }

            var moduleViewModel = _mapper.Map<ModuleViewModel>(module);
            return View(moduleViewModel);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var module = _db.Modules.GetByID(id);
            _db.Modules.Delete(module);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { courseId = module.CourseId });
        }
    }
}
