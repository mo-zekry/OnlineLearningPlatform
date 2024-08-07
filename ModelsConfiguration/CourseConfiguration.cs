using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.ModelsConfiguration;

public class CourseConfiguration : IEntityTypeConfiguration<Course> {
    public void Configure(EntityTypeBuilder<Course> builder) {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Price).IsRequired().HasDefaultValue(0);
        builder.Property(e => e.IsProgressLimited).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.Price).HasColumnType("decimal(18,2)");
    }
}