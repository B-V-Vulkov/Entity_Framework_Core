namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurePatientEntity(modelBuilder);

            ConfigureVisitationEntity(modelBuilder);

            ConfigureDiagnoseEntity(modelBuilder);

            ConfigureMedicamentEntity(modelBuilder);

            ConfigurePatientMedicamentEntity(modelBuilder);
        }

        private void ConfigurePatientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                        .HasKey(p => p.PatientId);

            modelBuilder.Entity<Patient>()
                        .HasMany(p => p.Visitations)
                        .WithOne(p => p.Patient);

            modelBuilder.Entity<Patient>()
                        .HasMany(p => p.Diagnoses)
                        .WithOne(p => p.Patient);

            modelBuilder.Entity<Patient>()
                        .HasMany(p => p.Prescriptions)
                        .WithOne(p => p.Patient);

            modelBuilder.Entity<Patient>()
                        .Property(p => p.FirstName)
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .IsRequired(true);

            modelBuilder.Entity<Patient>()
                        .Property(p => p.LastName)
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .IsRequired(true);

            modelBuilder.Entity<Patient>()
                        .Property(p => p.Address)
                        .HasMaxLength(250)
                        .IsUnicode(true);

            modelBuilder.Entity<Patient>()
                        .Property(p => p.Email)
                        .HasMaxLength(80)
                        .IsUnicode(false);
        }

        private void ConfigureVisitationEntity(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Visitation>()
                        .HasKey(v => v.VisitationId);

            modelBuilder.Entity<Visitation>()
                        .Property(v => v.Comments)
                        .HasMaxLength(250)
                        .IsUnicode();
        }

        private void ConfigureDiagnoseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnose>()
                        .HasKey(d => d.DiagnoseId);

            modelBuilder.Entity<Diagnose>()
                        .Property(d => d.Name)
                        .HasMaxLength(50)
                        .IsUnicode();

            modelBuilder.Entity<Diagnose>()
                        .Property(d => d.Comments)
                        .HasMaxLength(250)
                        .IsUnicode();
        }

        private void ConfigureMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>()
                        .HasKey(m => m.MedicamentId);

            modelBuilder.Entity<Medicament>()
                        .HasMany(m => m.Prescriptions)
                        .WithOne(m => m.Medicament);

            modelBuilder.Entity<Medicament>()
                        .Property(m => m.Name)
                        .HasMaxLength(50)
                        .IsUnicode();
        }

        private void ConfigurePatientMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>()
                        .HasKey(pm => new { pm.PatientId, pm.MedicamentId });

            modelBuilder.Entity<PatientMedicament>()
                        .HasOne(pm => pm.Patient)
                        .WithMany(pm => pm.Prescriptions)
                        .HasForeignKey(pm => pm.PatientId);

            modelBuilder.Entity<PatientMedicament>()
                        .HasOne(pm => pm.Medicament)
                        .WithMany(pm => pm.Prescriptions)
                        .HasForeignKey(pm => pm.MedicamentId);
        }
    }
}
