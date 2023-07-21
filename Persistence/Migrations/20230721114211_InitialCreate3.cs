using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(3607),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(1488));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(2778),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(685));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(9800),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(7898));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7847),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(6110));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7551),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(5829));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(4066),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(2715));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(969),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 676, DateTimeKind.Local).AddTicks(9924));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(1488),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(3607));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(685),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(2778));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(7898),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(9800));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(6110),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7847));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(5829),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7551));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(2715),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(4066));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 676, DateTimeKind.Local).AddTicks(9924),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(969));
        }
    }
}
