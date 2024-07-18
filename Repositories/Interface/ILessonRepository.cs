using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Class;

namespace OnlineLearningPlatform.Repositories.Interface;

public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
{
    public LessonRepository(ApplicationDbContext context)
        : base(context) { }
}
