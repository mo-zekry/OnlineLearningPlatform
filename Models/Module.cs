namespace OnlineLearningPlatform.Models;

public class Module
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
