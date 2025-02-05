using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbConfig
{
    public class LinkReportEntityConfig : IEntityTypeConfiguration<LinkReport>
    {
        public void Configure(EntityTypeBuilder<LinkReport> builder)
        {
            builder
           .ToTable("LinkReports", SchemaNames.LINK)
           .HasIndex(e => e.Id)
           .HasDatabaseName("IX_LinkReport_Id");

            builder
                .HasIndex(e => e.LinkId)
                .HasDatabaseName("IX_LinkId");
        }
    }
}
