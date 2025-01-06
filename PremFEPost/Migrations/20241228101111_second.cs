using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PremFEPost.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8e798cb6-0da5-4b5b-98c2-cf30e6d2c73f", null, "Initiator", "Initiator" },
                    { "bb0e8e1b-2032-4863-89cd-acf7bff7bc62", null, "Admin", "Admin" },
                    { "e0eb7a35-e8fe-4e55-9ecc-c346b217ff7a", null, "Approver", "Approver" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e798cb6-0da5-4b5b-98c2-cf30e6d2c73f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb0e8e1b-2032-4863-89cd-acf7bff7bc62");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0eb7a35-e8fe-4e55-9ecc-c346b217ff7a");
        }
    }
}
