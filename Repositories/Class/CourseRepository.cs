using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context)
        : base(context) { }

    public async Task<Course> GetWithAllRelated(int id)
    {
        return await _context
                .Courses.Include(c => c.Modules)
                .Include(c => c.Quizzes)
                .Include(c => c.Category)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new InvalidOperationException();
    }

    public async Task<Course> GetCourseWithModulesAndQuizzesAsync(int courseId)
    {
        return await _context
                .Courses.Include(c => c.Modules)
                .Include(c => c.Quizzes)
                .FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new InvalidOperationException();
    }
}
