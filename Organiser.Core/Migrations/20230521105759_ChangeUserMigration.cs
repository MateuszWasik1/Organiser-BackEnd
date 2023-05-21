using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Organiser.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a757bb4-e52f-4a21-99a6-98ea9f0be243");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3a6643d-1c5f-4b80-b0bc-94f5132e5eb3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b5088d8d-9801-4430-a739-787ffbf88b72", null, "User", "USER" },
                    { "bf085f36-729b-47d3-bd82-3c4372ff55cc", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5088d8d-9801-4430-a739-787ffbf88b72");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf085f36-729b-47d3-bd82-3c4372ff55cc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5a757bb4-e52f-4a21-99a6-98ea9f0be243", null, "Administrator", "ADMINISTRATOR" },
                    { "f3a6643d-1c5f-4b80-b0bc-94f5132e5eb3", null, "User", "USER" }
                });
        }
    }
}
