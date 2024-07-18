using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => new { e.CourseId, e.StudentId });
        builder.Property(e => e.EnrollmentDatetime).IsRequired().HasDefaultValueSql("getutcdate()");
        builder.HasOne(e => e.Course).WithMany(c => c.Enrollments).HasForeignKey(e => e.CourseId);
        builder.HasOne(e => e.Student).WithMany(s => s.Enrollments).HasForeignKey(e => e.StudentId);
    }
}
