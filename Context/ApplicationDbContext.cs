using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.ModelsConfiguration;

namespace OnlineLearningPlatform.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<QuizAnswer> QuizAnswers { get; set; }
    public DbSet<StudentQuizAttempt> StudentQuizAttempts { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<StudentLesson> StudentLessons { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public ApplicationDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionString = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build()
            .GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CourseConfiguration());
        builder.ApplyConfiguration(new ModuleConfiguration());
        builder.ApplyConfiguration(new LessonConfiguration());
        builder.ApplyConfiguration(new QuizConfiguration());
        builder.ApplyConfiguration(new QuizQuestionConfiguration());
        builder.ApplyConfiguration(new QuizAnswerConfiguration());
        builder.ApplyConfiguration(new StudentQuizAttemptConfiguration());
        builder.ApplyConfiguration(new EnrollmentConfiguration());
        builder.ApplyConfiguration(new StudentLessonConfiguration());
    }

public DbSet<OnlineLearningPlatform.ViewModels.CourseViewModel> CourseViewModel { get; set; } = default!;

public DbSet<OnlineLearningPlatform.ViewModels.CategoryViewModel> CategoryViewModel { get; set; } = default!;

public DbSet<OnlineLearningPlatform.ViewModels.ModuleViewModel> ModuleViewModel { get; set; } = default!;

public DbSet<OnlineLearningPlatform.ViewModels.QuizViewModel> QuizViewModel { get; set; } = default!;

public DbSet<OnlineLearningPlatform.ViewModels.QuizAnswerViewModel> QuizAnswerViewModel { get; set; } = default!;

public DbSet<OnlineLearningPlatform.Models.Category> Category { get; set; } = default!;

public DbSet<OnlineLearningPlatform.ViewModels.LessonViewModel> LessonViewModel { get; set; } = default!;

public DbSet<OnlineLearningPlatform.ViewModels.QuizQuestionViewModel> QuizQuestionViewModel { get; set; } = default!;
}
