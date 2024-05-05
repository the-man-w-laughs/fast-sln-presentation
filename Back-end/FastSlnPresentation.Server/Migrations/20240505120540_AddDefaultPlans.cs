using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastSlnPresentation.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "plans",
                columns: new[] { "plan_id", "description", "duration", "name", "price" },
                values: new object[,]
                {
                    { 1, "Подписка на месяц", 30, "Стандарт", 17m },
                    { 2, "Подписка на год", 365, "Стандарт годовая", 204m }
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "passwork_hash", "salt" },
                values: new object[] { new DateTime(2024, 5, 5, 15, 5, 40, 491, DateTimeKind.Local).AddTicks(7300), "1T7PcRihvH9uunaxv4YQS3675nKrgiytQGe0Q1tJpFA=", "SWihKZsAo+0rGKe2j0gSXw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "plans",
                keyColumn: "plan_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "plans",
                keyColumn: "plan_id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "passwork_hash", "salt" },
                values: new object[] { new DateTime(2024, 5, 5, 15, 1, 45, 758, DateTimeKind.Local).AddTicks(4778), "EDT5TDjkGQCnNkIDe9swaz/8vcnLCDtOh9iWKgkhX0o=", "h1mePoXseh1tUryFXSiI6w==" });
        }
    }
}
