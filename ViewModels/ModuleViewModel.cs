namespace OnlineLearningPlatform.ViewModels;

public class ModuleViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
}
