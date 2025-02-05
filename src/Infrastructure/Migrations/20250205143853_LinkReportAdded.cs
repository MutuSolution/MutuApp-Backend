using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkReportAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "LINK",
                table: "Links",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LinkReports",
                schema: "LINK",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkId = table.Column<int>(type: "int", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkReports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_UserName",
                schema: "LINK",
                table: "Links",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_LinkId",
                schema: "LINK",
                table: "LinkReports",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkReport_Id",
                schema: "LINK",
                table: "LinkReports",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkReports",
                schema: "LINK");

            migrationBuilder.DropIndex(
                name: "IX_Links_UserName",
                schema: "LINK",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "LINK",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
