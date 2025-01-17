using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QupaIntegrator.Migrations
{
    /// <inheritdoc />
    public partial class yuyuytrtyuuy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2039bf82-e2fc-4be4-8512-25a6d5cdc4a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d964bbf-2628-433d-b2ef-fde26ab2eb80");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d5a8906-dade-4896-93db-399e19a446ec");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "2039bf82-e2fc-4be4-8512-25a6d5cdc4a2", null, "Admin", "Admin" },
                    { "3d964bbf-2628-433d-b2ef-fde26ab2eb80", null, "Approver", "Approver" },
                    { "8d5a8906-dade-4896-93db-399e19a446ec", null, "Initiator", "Initiator" }
                });
        }
    }
}
