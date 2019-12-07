namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.Bet;

    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> bet)
        {
            bet
                .HasKey(b => b.BetId);

            bet
                .HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            bet
                .HasOne(b => b.Game)
                .WithMany(g => g.Bets)
                .HasForeignKey(fk => fk.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            bet
                .Property(a => a.Amount)
                .HasMaxLength(AmountMaxLength)
                .IsRequired(true);

            bet
                .Property(p => p.Prediction)
                .IsRequired(true);

            bet
                .Property(dt => dt.DataTime)
                .IsRequired(true);
        }
    }
}
