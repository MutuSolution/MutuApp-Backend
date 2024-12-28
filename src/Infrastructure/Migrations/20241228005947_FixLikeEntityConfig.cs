using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLikeEntityConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "Likes",
                newSchema: "LINK");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "LINK",
                table: "Likes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_Id",
                schema: "LINK",
                table: "Likes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserName",
                schema: "LINK",
                table: "Likes",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Likes_Id",
                schema: "LINK",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserName",
                schema: "LINK",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Likes",
                schema: "LINK",
                newName: "Likes");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Likes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
