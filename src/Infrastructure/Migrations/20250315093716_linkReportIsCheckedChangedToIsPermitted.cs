using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class linkReportIsCheckedChangedToIsPermitted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsChecked",
                schema: "LINK",
                table: "LinkReports",
                newName: "IsPermitted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPermitted",
                schema: "LINK",
                table: "LinkReports",
                newName: "IsChecked");
        }
    }
}
