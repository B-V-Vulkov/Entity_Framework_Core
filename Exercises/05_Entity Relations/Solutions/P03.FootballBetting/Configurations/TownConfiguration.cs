namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Town;

    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> town)
        {
            town
                .HasKey(t => t.TownId);

            town
                .HasOne(t => t.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(fk => fk.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            town
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired(true);
        }
    }
}
