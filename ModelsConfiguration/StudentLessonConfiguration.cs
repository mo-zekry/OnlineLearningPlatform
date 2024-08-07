using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

public class StudentLessonConfiguration : IEntityTypeConfiguration<StudentLesson> {
    public void Configure(EntityTypeBuilder<StudentLesson> builder) {
        builder.HasKey(e => new { e.StudentId, e.LessonId });
        builder.Property(e => e.CompletedDatetime).IsRequired().HasDefaultValueSql("getutcdate()");
        builder
            .HasOne(e => e.Student)
            .WithMany(s => s.StudentLessons)
            .HasForeignKey(e => e.StudentId);
        builder
            .HasOne(e => e.Lesson)
            .WithMany(l => l.StudentLessons)
            .HasForeignKey(e => e.LessonId);
    }
}