using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.ViewModels;

public class EnrollmentViewModel {
    public int CourseId { get; set; }

    [Display(Name = "Course Name")] public string CourseName { get; set; } = string.Empty;

    [Display(Name = "Enrollment Date")] public DateTime EnrolmentDateTime { get; set; }

    public IEnumerable<CourseViewModel> CoursesList { get; set; } = [];
}