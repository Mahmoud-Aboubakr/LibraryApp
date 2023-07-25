using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 25, 12, 25, 3, 179, DateTimeKind.Local).AddTicks(925),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 25, 12, 25, 3, 178, DateTimeKind.Local).AddTicks(5763),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 25, 11, 39, 45, 390, DateTimeKind.Local).AddTicks(6698));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 25, 12, 25, 3, 179, DateTimeKind.Local).AddTicks(925));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 25, 11, 39, 45, 390, DateTimeKind.Local).AddTicks(6698),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 25, 12, 25, 3, 178, DateTimeKind.Local).AddTicks(5763));
        }
    }
}
