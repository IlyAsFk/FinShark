using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2155e2f2-8550-47e5-91a3-893d5c639697");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d92574f-3a7f-4f22-9a98-6466a180e306");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "64e2dc52-3d47-4834-8276-6585be74a5c2", "User", "USER" },
                    { "2", "ae41bd01-cd92-4815-bacf-7c6b75c0d515", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2155e2f2-8550-47e5-91a3-893d5c639697", "9b0789b0-bb00-4336-8d83-eca860e027b4", "User", "USER" },
                    { "7d92574f-3a7f-4f22-9a98-6466a180e306", "fbfc34c1-c04a-413f-9ad9-0cf2dcd816dc", "Admin", "ADMIN" }
                });
        }
    }
}
