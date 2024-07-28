using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.ViewModels;

public class CourseViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; } = 0;

    [Required]
    [StringLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public bool IsProgressLimited { get; set; } = false;
    public int CategoryId { get; set; }
}
