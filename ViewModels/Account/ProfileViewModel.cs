using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.ViewModels.Account;

public class ProfileViewModel {
    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = "";

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = "";

    [Display(Name = "Profile Picture")]
    [DataType(DataType.Upload)]
    public IFormFile? ProfilePictureUrl { get; set; }

    public string? ProfilePictureBase64 { get; set; }
}