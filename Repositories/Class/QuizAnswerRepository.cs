using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class QuizAnswerRepository : GenericRepository<QuizAnswer>, IQuizAnswerRepository {
    public QuizAnswerRepository(ApplicationDbContext context) : base(context) { }
}