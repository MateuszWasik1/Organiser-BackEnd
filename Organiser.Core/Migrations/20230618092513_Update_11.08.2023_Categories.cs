using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Organiser.Core.Migrations
{
    /// <inheritdoc />
    public partial class Update_11082023_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63c3d7ce-1009-4f63-a539-52acb55df107");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c44cc98-b803-41a3-ab70-45b3f0bf9eb7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9902aefd-596f-4361-b4c6-590c5aaae6a1", null, "Administrator", "ADMINISTRATOR" },
                    { "a14d1788-3b46-4b5d-8180-dfab5fba4f05", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9902aefd-596f-4361-b4c6-590c5aaae6a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a14d1788-3b46-4b5d-8180-dfab5fba4f05");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "63c3d7ce-1009-4f63-a539-52acb55df107", null, "Administrator", "ADMINISTRATOR" },
                    { "9c44cc98-b803-41a3-ab70-45b3f0bf9eb7", null, "User", "USER" }
                });
        }
    }
}
