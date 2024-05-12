using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastSlnPresentation.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTextField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "plans",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "plans",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "passwork_hash", "salt" },
                values: new object[] { new DateTime(2024, 5, 12, 22, 45, 5, 236, DateTimeKind.Local).AddTicks(3217), "Q0NgcFYKIt9iElgEdR0j4UWUpJ1yUHJMN9kSKW57z2Q=", "/IYF//dXJYzDjzljXticqQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "plans",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "plans",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "passwork_hash", "salt" },
                values: new object[] { new DateTime(2024, 5, 5, 17, 29, 29, 669, DateTimeKind.Local).AddTicks(184), "0+dhVcuwP29tJJ9Ja0MtBCQt5B791zvO1MKvi9Y+/kY=", "mYk/m7klzRm9DitqP7SWrA==" });
        }
    }
}
