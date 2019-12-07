namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Data.Models;
    using static Data.DataValidations.Country;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> country)
        {
            country
                .HasKey(c => c.CountryId);

            country
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired(true)
                .IsUnicode(true);
        }
    }
}
