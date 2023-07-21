using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Vacations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Vacations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Vacations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReturnOrderDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "ReturnOrderDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ReturnOrderDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(1488),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 558, DateTimeKind.Local).AddTicks(131));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "ReturnedOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Publishers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Publishers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Publishers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(685),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(9248));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Payrolls",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(7898),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(4477));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(6110),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(2381));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(5829),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(2131));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Borrows",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Borrows",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Borrows",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BookOrderDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "BookOrderDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "BookOrderDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(2715),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 556, DateTimeKind.Local).AddTicks(8825));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "BannedCustomers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Authors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Authors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Authors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 676, DateTimeKind.Local).AddTicks(9924),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 556, DateTimeKind.Local).AddTicks(5787));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Attendence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReturnOrderDetails");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "ReturnOrderDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ReturnOrderDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReturnedOrders");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "ReturnedOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ReturnedOrders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BookOrderDetails");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "BookOrderDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "BookOrderDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BannedCustomers");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "BannedCustomers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "BannedCustomers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Attendence");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Attendence");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Attendence");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 558, DateTimeKind.Local).AddTicks(131),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(1488));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(9248),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 678, DateTimeKind.Local).AddTicks(685));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(4477),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(7898));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 24, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(2381),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 24, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(6110));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(2131),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(5829));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 556, DateTimeKind.Local).AddTicks(8825),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 677, DateTimeKind.Local).AddTicks(2715));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 556, DateTimeKind.Local).AddTicks(5787),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 21, 14, 39, 52, 676, DateTimeKind.Local).AddTicks(9924));
        }
    }
}
