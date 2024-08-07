using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.ViewModels {
    public class PaymentViewModel {
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Required] [Display(Name = "Amount")] public decimal Amount { get; set; }

        public string StripePublishableKey { get; set; } = string.Empty;
    }
}