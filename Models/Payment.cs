using OnlineLearningPlatform.Context.Identity;

namespace OnlineLearningPlatform.Models;

public class Payment {
    public int Id { get; set; }
    public string StripePaymentId { get; set; } = null!;
    public string StudentId { get; set; } = null!;
    public int CourseId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }

    public ApplicationUser Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}