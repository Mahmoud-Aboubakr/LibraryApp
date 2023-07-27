﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(7288),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 286, DateTimeKind.Local).AddTicks(7749));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(3515),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 286, DateTimeKind.Local).AddTicks(935));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(2370),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 285, DateTimeKind.Local).AddTicks(9008));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(8851),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 285, DateTimeKind.Local).AddTicks(1273));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5784),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 30, 9, 37, 9, 284, DateTimeKind.Local).AddTicks(5395));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5397),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 284, DateTimeKind.Local).AddTicks(4685));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(155),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 283, DateTimeKind.Local).AddTicks(1042));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpLeavingTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4213),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 282, DateTimeKind.Local).AddTicks(644));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpArrivalTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(3802),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 281, DateTimeKind.Local).AddTicks(9879));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4389),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 282, DateTimeKind.Local).AddTicks(1201));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 286, DateTimeKind.Local).AddTicks(7749),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(7288));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 286, DateTimeKind.Local).AddTicks(935),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(3515));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 285, DateTimeKind.Local).AddTicks(9008),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 119, DateTimeKind.Local).AddTicks(2370));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 285, DateTimeKind.Local).AddTicks(1273),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(8851));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 9, 37, 9, 284, DateTimeKind.Local).AddTicks(5395),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 30, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5784));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 284, DateTimeKind.Local).AddTicks(4685),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(5397));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 283, DateTimeKind.Local).AddTicks(1042),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 118, DateTimeKind.Local).AddTicks(155));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpLeavingTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 282, DateTimeKind.Local).AddTicks(644),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4213));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpArrivalTime",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 281, DateTimeKind.Local).AddTicks(9879),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(3802));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 27, 9, 37, 9, 282, DateTimeKind.Local).AddTicks(1201),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 27, 12, 23, 40, 117, DateTimeKind.Local).AddTicks(4389));
        }
    }
}
