using OnlineLearningPlatform.Context.Identity;

namespace OnlineLearningPlatform.Models;

public class StudentLesson
{
    public string StudentId { get; set; } = default!;
    public int LessonId { get; set; }
    public DateTime CompletedDatetime { get; set; } = DateTime.UtcNow;

    public Student Student { get; set; } = default!;
    public Lesson Lesson { get; set; } = default!;
}