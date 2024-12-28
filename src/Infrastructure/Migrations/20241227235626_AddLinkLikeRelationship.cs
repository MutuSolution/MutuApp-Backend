using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkLikeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Likes_LinkId",
                table: "Likes",
                column: "LinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Links_LinkId",
                table: "Likes",
                column: "LinkId",
                principalSchema: "LINK",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Links_LinkId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_LinkId",
                table: "Likes");
        }
    }
}
