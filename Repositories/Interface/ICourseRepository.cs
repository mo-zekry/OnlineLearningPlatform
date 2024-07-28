using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Repositories.Interface;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course> GetWithAllRelated(int id);
    Task<Course> GetCourseWithModulesAndQuizzesAsync(int courseId);
}
