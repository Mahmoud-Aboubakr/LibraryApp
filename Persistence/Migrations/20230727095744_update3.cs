using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 771, DateTimeKind.Local).AddTicks(2974),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(7288));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 770, DateTimeKind.Local).AddTicks(7636),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(3515));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 770, DateTimeKind.Local).AddTicks(6003),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(2370));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 770, DateTimeKind.Local).AddTicks(733),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(8851));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 12, 57, 43, 769, DateTimeKind.Local).AddTicks(6479),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 30, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5784));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 769, DateTimeKind.Local).AddTicks(6054),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5397));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(9736),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(155));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpLeavingTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(3813),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4213));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpArrivalTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(3065),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(3802));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(4103),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4389));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(7288),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 771, DateTimeKind.Local).AddTicks(2974));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(3515),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 770, DateTimeKind.Local).AddTicks(7636));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(2370),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 770, DateTimeKind.Local).AddTicks(6003));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(8851),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 770, DateTimeKind.Local).AddTicks(733));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5784),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 30, 12, 57, 43, 769, DateTimeKind.Local).AddTicks(6479));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5397),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 769, DateTimeKind.Local).AddTicks(6054));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(155),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(9736));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpLeavingTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4213),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(3813));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpArrivalTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(3802),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(3065));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4389),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 57, 43, 768, DateTimeKind.Local).AddTicks(4103));
        }
    }
}
