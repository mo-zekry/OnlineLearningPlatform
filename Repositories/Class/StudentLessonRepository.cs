using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class StudentLessonRepository : GenericRepository<StudentLesson>, IStudentLessonRepository
{
    public StudentLessonRepository(ApplicationDbContext context)
        : base(context) { }
}
