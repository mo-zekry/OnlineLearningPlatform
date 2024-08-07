using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class QuizRepository : GenericRepository<Quiz>, IQuizRepository {
    public QuizRepository(ApplicationDbContext context)
        : base(context) { }
}