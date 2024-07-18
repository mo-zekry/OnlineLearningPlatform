namespace OnlineLearningPlatform.ViewModels;

public class StudentLessonViewModel
{
    public string StudentId { get; set; } = default!;
    public int LessonId { get; set; }
    public DateTime CompletedDatetime { get; set; } = DateTime.UtcNow;
    public string StudentName { get; set; } = string.Empty;
    public string LessonName { get; set; } = string.Empty;
}
