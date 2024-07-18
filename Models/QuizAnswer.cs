namespace OnlineLearningPlatform.Models;

public class QuizAnswer
{
    public int Id { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; } = false;
    public int QuestionId { get; set; }

    public QuizQuestion QuizQuestion { get; set; } = default!;
}