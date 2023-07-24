using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 86, DateTimeKind.Local).AddTicks(718),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(3607));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 85, DateTimeKind.Local).AddTicks(8408),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(2778));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 85, DateTimeKind.Local).AddTicks(1744),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(9800));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 11, 34, 15, 84, DateTimeKind.Local).AddTicks(5742),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7847));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 84, DateTimeKind.Local).AddTicks(4852),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7551));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 83, DateTimeKind.Local).AddTicks(7896),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(4066));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 83, DateTimeKind.Local).AddTicks(1473),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(969));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(3607),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 86, DateTimeKind.Local).AddTicks(718));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 330, DateTimeKind.Local).AddTicks(2778),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 85, DateTimeKind.Local).AddTicks(8408));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(9800),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 85, DateTimeKind.Local).AddTicks(1744));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7847),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 27, 11, 34, 15, 84, DateTimeKind.Local).AddTicks(5742));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(7551),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 84, DateTimeKind.Local).AddTicks(4852));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(4066),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 83, DateTimeKind.Local).AddTicks(7896));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 21, 14, 42, 11, 329, DateTimeKind.Local).AddTicks(969),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 24, 11, 34, 15, 83, DateTimeKind.Local).AddTicks(1473));
        }
    }
}
