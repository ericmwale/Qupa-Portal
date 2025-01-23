using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QupaIntegrator.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00a3291c-14a7-4cba-97e5-d0ea98cc1c79");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a6e3cbd-65a4-4da2-ba05-52d9f65e059d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d91ecfe-a945-4e75-953a-482bd36f9a53");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5586ec68-56a3-4b04-b977-07f9f2bbdecd", null, "Initiator", "Initiator" },
                    { "dcfcc738-b620-4dc3-aa45-e0e6ee8b9b2b", null, "Admin", "Admin" },
                    { "ef11b5cc-61a6-43b1-89e2-06ad20f67991", null, "Approver", "Approver" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5586ec68-56a3-4b04-b977-07f9f2bbdecd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcfcc738-b620-4dc3-aa45-e0e6ee8b9b2b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef11b5cc-61a6-43b1-89e2-06ad20f67991");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00a3291c-14a7-4cba-97e5-d0ea98cc1c79", null, "Admin", "Admin" },
                    { "0a6e3cbd-65a4-4da2-ba05-52d9f65e059d", null, "Approver", "Approver" },
                    { "8d91ecfe-a945-4e75-953a-482bd36f9a53", null, "Initiator", "Initiator" }
                });
        }
    }
}
