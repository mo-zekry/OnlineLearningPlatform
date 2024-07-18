using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class StudentQuizAttemptRepository : GenericRepository<StudentQuizAttempt>, IStudentQuizAttemptRepository {
    public StudentQuizAttemptRepository(ApplicationDbContext context) : base(context) { }
}