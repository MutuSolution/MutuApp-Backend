using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbConfig;

public class LikeEntityConfig : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder
            .ToTable("Likes", SchemaNames.LINK)
            .HasIndex(e => e.Id)
            .HasDatabaseName("IX_Likes_Id");

        builder
            .HasIndex(e => e.UserName)
            .HasDatabaseName("IX_Likes_UserName");
    }
}