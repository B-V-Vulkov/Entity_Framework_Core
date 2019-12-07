namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Game;

    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> game)
        {
            game
                .HasKey(g => g.GameId);

            game
                .HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(fk => fk.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            game
                .HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(fk => fk.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            game
                .Property(htg => htg.HomeTeamGoals)
                .HasMaxLength(HomeTeamGoalsMaxLength)
                .IsRequired(true);

            game
                .Property(atg => atg.AwayTeamGoals)
                .HasMaxLength(AwayTeamGoalsMaxLength)
                .IsRequired(true);

            game
                .Property(dt => dt.DateTime)
                .IsRequired(true);

            game
                .Property(htbr => htbr.HomeTeamBetRate)
                .HasMaxLength(HomeTeamBetRateMaxLength)
                .IsRequired(true);

            game
                .Property(atbr => atbr.AwayTeamBetRate)
                .HasMaxLength(AwayTeamBetRateMaxLength)
                .IsRequired(true);

            game
                .Property(dbr => dbr.DrawBetRate)
                .HasMaxLength(DrawBetRateMaxLength)
                .IsRequired(true);

            game
                .Property(r => r.Result)
                .HasMaxLength(ResultMaxLength)
                .IsRequired(true);
        }
    }
}
