namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Team;

    class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> team)
        {
            team
                .HasKey(t => t.TeamId);

            team
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(fk => fk.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            team
                .HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(fk => fk.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            team
                .HasOne(t => t.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(fk => fk.TownId)
                .OnDelete(DeleteBehavior.Restrict);

            team
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired(true)
                .IsUnicode(true);

            team
                .Property(l => l.LogoUrl)
                .HasMaxLength(LogoUrlrMaxLength)
                .IsRequired(true)
                .IsUnicode(true);

            team
                .Property(i => i.Initials)
                .HasMaxLength(InitialsMaxLength)
                .IsFixedLength(true)
                .IsRequired(true)
                .IsUnicode(true);

            team
                .Property(b => b.Budget)
                .IsRequired(true);
        }
    }
}
