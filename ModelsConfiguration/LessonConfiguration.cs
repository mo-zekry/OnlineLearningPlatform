using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson> {
    public void Configure(EntityTypeBuilder<Lesson> builder) {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Number).IsRequired();
        builder.Property(e => e.VideoUrl).IsRequired().HasMaxLength(200);
        builder.Property(e => e.LessonDetails).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.CourseOrder).IsRequired();
        builder.HasOne(e => e.Module).WithMany(m => m.Lessons).HasForeignKey(e => e.ModuleId);
    }
}