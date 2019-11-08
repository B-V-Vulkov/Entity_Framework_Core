namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Color;

    public class ColorConfigurations : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> color)
        {
            color
                .HasKey(c => c.ColorId);

            color
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired(true)
                .IsUnicode(true);
        }
    }
}
