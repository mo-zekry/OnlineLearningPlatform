using OnlineLearningPlatform.Context.Identity;

namespace OnlineLearningPlatform.Models;

public class Enrollment
{
    public int CourseId { get; set; }
    public string StudentId { get; set; } = default!;
    public DateTime EnrollmentDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedDatetime { get; set; }
    public Course Course { get; set; } = default!;
    public Student Student { get; set; } = default!;
}
