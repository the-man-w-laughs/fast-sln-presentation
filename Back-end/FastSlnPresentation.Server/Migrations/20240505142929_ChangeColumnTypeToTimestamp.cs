using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastSlnPresentation.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnTypeToTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "refresh_tokens",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "passwork_hash", "salt" },
                values: new object[] { new DateTime(2024, 5, 5, 17, 29, 29, 669, DateTimeKind.Local).AddTicks(184), "0+dhVcuwP29tJJ9Ja0MtBCQt5B791zvO1MKvi9Y+/kY=", "mYk/m7klzRm9DitqP7SWrA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "passwork_hash", "salt" },
                values: new object[] { new DateTime(2024, 5, 5, 15, 5, 40, 491, DateTimeKind.Local).AddTicks(7300), "1T7PcRihvH9uunaxv4YQS3675nKrgiytQGe0Q1tJpFA=", "SWihKZsAo+0rGKe2j0gSXw==" });
        }
    }
}
