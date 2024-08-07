using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class LessonRepository : GenericRepository<Lesson>, ILessonRepository {
    public LessonRepository(ApplicationDbContext context)
        : base(context) { }
}