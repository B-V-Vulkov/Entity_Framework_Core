namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class StudentSystemContext : DbContext
    {
        DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }



    }
}
