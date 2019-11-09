namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.PlayerStatistic;

    public class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> playerStatistic)
        {
            playerStatistic
                .HasKey(key => new { key.PlayerId, key.GameId});

            playerStatistic
                .HasOne(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(fk => fk.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            playerStatistic
                .HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(fk => fk.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            playerStatistic
                .Property(sg => sg.ScoredGoals)
                .HasMaxLength(ScoredGoalsMaxLength)
                .IsRequired(true);

            playerStatistic
                .Property(a => a.Assists)
                .HasMaxLength(AssistsMaxLength)
                .IsRequired(true);

            playerStatistic
                .Property(mp => mp.MinutesPlayed)
                .HasMaxLength(MinutesPlayedMaxLength)
                .IsRequired(true);
        }
    }
}
