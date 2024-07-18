namespace OnlineLearningPlatform.Models;

public class QuizQuestion
{
    public int Id { get; set; }
    public string QuestionTitle { get; set; } = string.Empty;
    public int QuizId { get; set; }

    public Quiz Quiz { get; set; } = default!;
    public ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();
}
