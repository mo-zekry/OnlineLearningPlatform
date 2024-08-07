namespace OnlineLearningPlatform.ViewModels;

public class StudentQuizAttemptViewModel {
    public string StudentId { get; set; } = default!;
    public int QuizId { get; set; }
    public DateTime AttemptDatetime { get; set; }
    public int ScoreAchieved { get; set; }
}