using OnlineLearningPlatform.Repositories.Class;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICourseRepository Courses { get; }
    ICategoryRepository Categories { get; }
    IModuleRepository Modules { get; }
    ILessonRepository Lessons { get; }
    IQuizRepository Quizzes { get; }
    IQuizQuestionRepository QuizQuestions { get; }
    IQuizAnswerRepository QuizAnswers { get; }
    IStudentRepository Students { get; }
    IEnrollmentRepository Enrollments { get; }
    IStudentQuizAttemptRepository StudentQuizAttempts { get; }
    IStudentLessonRepository StudentLessons { get; }
    public int SaveChanges();
}
