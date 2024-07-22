namespace OnlineLearningPlatform.Models;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsProgressLimited { get; set; } = false;
    public int CategoryId { get; set; }

    public Category Category { get; set; } = new Category();
    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public IEnumerable<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
