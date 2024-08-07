using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.ViewModels;

public class ModuleViewModel {
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

    [Required] public int Number { get; set; }

    public int CourseId { get; set; }
}