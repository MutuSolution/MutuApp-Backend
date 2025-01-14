using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfig;

internal class LinkEntityConfig : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder
            .ToTable("Links", SchemaNames.LINK)
            .HasIndex(e => e.Title)
            .HasDatabaseName("IX_Links_Title");

        builder
            .HasIndex(e => e.Description)
            .HasDatabaseName("IX_Links_Description");

        // UserName için indeks ekleniyor
        builder
            .HasIndex(e => e.UserName)
            .HasDatabaseName("IX_Links_UserName");
    }
}
