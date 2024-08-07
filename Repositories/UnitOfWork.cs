using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Repositories.Class;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories;

public class UnitOfWork : IUnitOfWork {
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context) {
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

    public ICourseRepository Courses { get; }
    public IModuleRepository Modules { get; }
    public ILessonRepository Lessons { get; }
    public IQuizRepository Quizzes { get; }
    public IQuizQuestionRepository QuizQuestions { get; }
    public IQuizAnswerRepository QuizAnswers { get; }
    public IStudentRepository Students { get; }
    public IEnrollmentRepository Enrollments { get; }
    public IStudentQuizAttemptRepository StudentQuizAttempts { get; }
    public IStudentLessonRepository StudentLessons { get; }

    public ICategoryRepository Categories { get; }

    public int SaveChanges() {
        return _context.SaveChanges();
    }

    public void Dispose() {
        _context.Dispose();
    }
}