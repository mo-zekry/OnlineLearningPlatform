using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Repositories.Class;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Courses = new CourseRepository(_context);
        Categories = new CategoryRepository(_context);
        Modules = new ModuleRepository(_context);
        Lessons = new LessonRepository(_context);
        Quizzes = new QuizRepository(_context);
        QuizQuestions = new QuizQuestionRepository(_context);
        QuizAnswers = new QuizAnswerRepository(_context);
        Students = new StudentRepository(_context);
        Enrollments = new EnrollmentRepository(_context);
        StudentQuizAttempts = new StudentQuizAttemptRepository(_context);
        StudentLessons = new StudentLessonRepository(_context);
    }

    public ICourseRepository Courses { get; private set; }
    public IModuleRepository Modules { get; private set; }
    public ILessonRepository Lessons { get; private set; }
    public IQuizRepository Quizzes { get; private set; }
    public IQuizQuestionRepository QuizQuestions { get; private set; }
    public IQuizAnswerRepository QuizAnswers { get; private set; }
    public IStudentRepository Students { get; private set; }
    public IEnrollmentRepository Enrollments { get; private set; }
    public IStudentQuizAttemptRepository StudentQuizAttempts { get; private set; }
    public IStudentLessonRepository StudentLessons { get; private set; }

    public ICategoryRepository Categories { get; private set; }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
