using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class QuizQuestionRepository : GenericRepository<QuizQuestion>, IQuizQuestionRepository {
    public QuizQuestionRepository(ApplicationDbContext context)
        : base(context) { }
}