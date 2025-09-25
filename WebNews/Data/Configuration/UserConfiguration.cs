using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebNews.Models;

namespace WebNews.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.News)
            .WithOne(x => x.User);

        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Password)
            .IsRequired();
    }
}