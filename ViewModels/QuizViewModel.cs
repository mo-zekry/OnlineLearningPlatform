namespace OnlineLearningPlatform.ViewModels;

public class QuizViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
    public int CourseOrder { get; set; }
    public int MinPassScore { get; set; } = 0;
    public bool IsPassRequired { get; set; } = false;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
}
