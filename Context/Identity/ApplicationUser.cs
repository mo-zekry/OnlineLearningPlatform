using Microsoft.AspNetCore.Identity;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Context.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public ICollection<StudentQuizAttempt> StudentQuizAttempts { get; set; } =
        new List<StudentQuizAttempt>();

    public IEnumerable<Enrollment>? Enrollments { get; set; } = new List<Enrollment>();

    public ICollection<StudentLesson> StudentLessons { get; set; } = new List<StudentLesson>();
}
