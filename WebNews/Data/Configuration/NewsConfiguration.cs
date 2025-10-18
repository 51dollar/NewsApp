using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebNews.Models.Entities;

namespace WebNews.Data.Configuration;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.News)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(x => x.Subtitle)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(n => n.Author)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(x => x.DateTime)
            .HasColumnType("date");
    }
}