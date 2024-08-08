using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.ViewModels;

public class QuizViewModel {
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

    [Required] public int Number { get; set; }

    [Required] public int CourseOrder { get; set; }

    [Required] public int MinPassScore { get; set; } = 0;

    [Required] public bool IsPassRequired { get; set; } = false;

    [Required] public int CourseId { get; set; }
}