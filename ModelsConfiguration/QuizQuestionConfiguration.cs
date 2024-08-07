using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion> {
    public void Configure(EntityTypeBuilder<QuizQuestion> builder) {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.QuestionTitle).IsRequired().HasMaxLength(200);
        builder.HasOne(e => e.Quiz).WithMany(q => q.QuizQuestions).HasForeignKey(e => e.QuizId);
    }
}