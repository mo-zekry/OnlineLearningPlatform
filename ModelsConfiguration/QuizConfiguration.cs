using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Number).IsRequired();
        builder.Property(e => e.CourseOrder).IsRequired();
        builder.Property(e => e.MinPassScore).IsRequired().HasDefaultValue(0);
        builder.Property(e => e.IsPassRequired).IsRequired().HasDefaultValue(false);
        builder.HasOne(e => e.Course).WithMany(c => c.Quizzes).HasForeignKey(e => e.CourseId);
    }
}
