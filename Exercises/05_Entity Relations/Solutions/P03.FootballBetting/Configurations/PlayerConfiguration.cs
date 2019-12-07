namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Player;

    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> player)
        {
            player
                .HasKey(p => p.PlayerId);

            player
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(fk => fk.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            player
                .HasOne(p => p.Position)
                .WithMany(p => p.Players)
                .HasForeignKey(fk => fk.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            player
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired(true);

            player
                .Property(sn => sn.SquadNumber)
                .HasMaxLength(SquadNumberMaxLength)
                .IsRequired(true);

            player
                .Property(i => i.IsInjured)
                .IsRequired(true);
        }
    }
}
