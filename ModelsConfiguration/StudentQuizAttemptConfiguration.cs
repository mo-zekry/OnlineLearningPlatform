using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

public class StudentQuizAttemptConfiguration : IEntityTypeConfiguration<StudentQuizAttempt> {
    public void Configure(EntityTypeBuilder<StudentQuizAttempt> builder) {
        builder.HasKey(e => new {
            e.StudentId,
            e.QuizId,
            e.AttemptDatetime
        });

        builder.Property(e => e.ScoreAchieved).IsRequired().HasDefaultValue(0);

        builder
            .HasOne(e => e.Student)
            .WithMany(s => s.StudentQuizAttempts)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Quiz)
            .WithMany(q => q.StudentQuizAttempts)
            .HasForeignKey(e => e.QuizId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}