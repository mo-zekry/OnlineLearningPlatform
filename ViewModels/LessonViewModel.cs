namespace OnlineLearningPlatform.ViewModels;

public class LessonViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public string LessonDetails { get; set; } = string.Empty;
    public int CourseOrder { get; set; }
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
}
