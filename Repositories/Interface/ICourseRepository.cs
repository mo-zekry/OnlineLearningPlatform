using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Class;

namespace OnlineLearningPlatform.Repositories.Interface;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context)
        : base(context) { }

    public async Task<Course> GetCourseWithModulesAndQuizzesAsync(int courseId)
    {
        return await _context
                .Courses.Include(c => c.Modules)
                .Include(c => c.Quizzes)
                .FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new InvalidOperationException();
    }
}
