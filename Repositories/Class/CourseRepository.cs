using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class CourseRepository : GenericRepository<Course>, ICourseRepository {
    public CourseRepository(ApplicationDbContext context)
        : base(context) { }
}