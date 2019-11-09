namespace P03_FootballBetting.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Data.Models;
    using static Data.DataValidations.User;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user
                .HasKey(u => u.UserId);

            user
                .Property(u => u.Username)
                .HasMaxLength(UsernameMaxLength)
                .IsRequired(true)
                .IsUnicode(true);

            user
                .Property(p => p.Password)
                .HasMaxLength(PasswordMaxLength)
                .IsRequired(true);

            user
                .Property(e => e.Email)
                .HasMaxLength(EmailMaxLength)
                .IsRequired(true)
                .IsUnicode(true);

            user
                .Property(n => n.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired(true);

            user
                .Property(b => b.Balance)
                .HasMaxLength(BalanceMaxLength)
                .IsRequired(true);
        }
    }
}
