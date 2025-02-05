using Domain;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        ApplicationRoleClaim,
        IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Decimal türler için varsayılan ayar
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }

        // Link ve Like ilişkisi
        builder.Entity<Like>()
            .HasOne(like => like.Link)
            .WithMany(link => link.Likes)
            .HasForeignKey(like => like.LinkId)
            .OnDelete(DeleteBehavior.Cascade); // Silme davranışı

        // Varsayılan Identity yapılandırmaları
        base.OnModelCreating(builder);

        // Fluent API konfigürasyonları
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }


    public DbSet<Link> Links => Set<Link>();
    public DbSet<Like> Likes { get; set; }
    public DbSet<LinkReport> LinkReports { get; set; }

}
