using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.ViewModels.Account
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";

        [Url]
        [Display(Name = "Profile Picture URL")]
        public string? ProfilePictureUrl { get; set; }
    }
}
