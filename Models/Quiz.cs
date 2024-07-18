namespace OnlineLearningPlatform.Models;

public class Quiz {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
    public int CourseOrder { get; set; }
    public int MinPassScore { get; set; } = 0;
    public bool IsPassRequired { get; set; } = false;
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    public ICollection<StudentQuizAttempt> StudentQuizAttempts { get; set; } = new List<StudentQuizAttempt>();
}