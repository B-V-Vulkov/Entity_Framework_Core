namespace P01_StudentSystem.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Course;

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> course)
        {
            course
                .HasKey(c => c.CourseId);

            course
                .HasMany(s => s.StudentsEnrolled)
                .WithOne(c => c.Course);

            course
                .HasMany(h => h.HomeworkSubmissions)
                .WithOne(c => c.Course);

            course
                .HasMany(r => r.Resources)
                .WithOne(c => c.Course);
  
            course
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsUnicode(true)
                .IsRequired(false);

            course
                .Property(d => d.Description)
                .IsUnicode(true)
                .IsRequired(false);
        }
    }
}
