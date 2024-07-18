namespace OnlineLearningPlatform.ViewModels;

public class EnrollmentViewModel
{
    public int CourseId { get; set; }
    public string StudentId { get; set; } = default!;
    public DateTime EnrollmentDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedDatetime { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
}
