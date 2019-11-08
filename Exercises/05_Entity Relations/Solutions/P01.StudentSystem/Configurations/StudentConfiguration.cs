namespace P01_StudentSystem.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Student;

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> student)
        {
            student
                .HasKey(s => s.StudentId);

            student
                .HasMany(h => h.HomeworkSubmissions)
                .WithOne(s => s.Student);

            student
                .HasMany(c => c.CourseEnrollments)
                .WithOne(s => s.Student);

            student
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsUnicode(true)
                .IsRequired(true);

            student
                .Property(pn => pn.PhoneNumber)
                .HasMaxLength(PhoneNumberMaxLength)
                .IsFixedLength()
                .IsRequired(false);
        }
    }
}
