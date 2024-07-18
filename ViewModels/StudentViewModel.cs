namespace OnlineLearningPlatform.ViewModels;

public class StudentViewModel {
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? ProfilePictureUrl { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
}