namespace OnlineLearningPlatform.ViewModels;

public class QuizQuestionViewModel
{
    public int Id { get; set; }
    public string QuestionTitle { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public string QuizName { get; set; } = string.Empty;
}
