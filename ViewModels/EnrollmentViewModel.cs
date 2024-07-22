namespace OnlineLearningPlatform.ViewModels;

public class EnrollmentViewModel
{
    public int CourseId { get; set; }
    public string StudentId { get; set; } = default!;
    public DateTime EnrollmentDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedDatetime { get; set; }
}
