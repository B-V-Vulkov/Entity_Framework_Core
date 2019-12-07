namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Position;

    class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> position)
        {
            position
                .HasKey(p => p.PositionId);

            position
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired();
        }
    }
}
