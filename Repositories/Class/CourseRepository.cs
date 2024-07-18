using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public interface ICourseRepository : IGenericRepository<Course> {
    Task<Course> GetCourseWithModulesAndQuizzesAsync(int courseId);
}