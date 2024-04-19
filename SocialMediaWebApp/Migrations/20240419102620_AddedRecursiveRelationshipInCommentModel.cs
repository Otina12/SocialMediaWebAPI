using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecursiveRelationshipInCommentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fd70ad1-afb1-4984-b8c9-6c8db3d31c0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5333acc9-98a9-4f79-b20c-b8973fbae7b3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8901ea25-5c4e-40f4-bfde-2e3b66ff4bb5", null, "Admin", "ADMIN" },
                    { "b09fa0fb-530b-4bea-89fb-31ff4fc56de2", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IsReplyToId",
                table: "Comments",
                column: "IsReplyToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_IsReplyToId",
                table: "Comments",
                column: "IsReplyToId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_IsReplyToId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IsReplyToId",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8901ea25-5c4e-40f4-bfde-2e3b66ff4bb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b09fa0fb-530b-4bea-89fb-31ff4fc56de2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fd70ad1-afb1-4984-b8c9-6c8db3d31c0a", null, "Admin", "ADMIN" },
                    { "5333acc9-98a9-4f79-b20c-b8973fbae7b3", null, "User", "USER" }
                });
        }
    }
}
