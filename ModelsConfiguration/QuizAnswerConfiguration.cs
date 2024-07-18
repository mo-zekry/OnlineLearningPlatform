using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer> {
    public void Configure(EntityTypeBuilder<QuizAnswer> builder) {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AnswerText).IsRequired().HasMaxLength(200);
        builder.Property(e => e.IsCorrect).IsRequired().HasDefaultValue(false);
        builder.HasOne(e => e.QuizQuestion)
            .WithMany(q => q.QuizAnswers)
            .HasForeignKey(e => e.QuestionId);
    }
}
