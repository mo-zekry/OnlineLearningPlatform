using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Class;

namespace OnlineLearningPlatform.Repositories.Interface;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course> GetWithAllRelated(int id);
    Task<Course> GetCourseWithModulesAndQuizzesAsync(int courseId);
}
