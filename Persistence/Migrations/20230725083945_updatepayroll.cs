using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updatepayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 25, 11, 39, 45, 390, DateTimeKind.Local).AddTicks(5592),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 25, 11, 21, 17, 567, DateTimeKind.Local).AddTicks(4403));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 25, 11, 21, 17, 567, DateTimeKind.Local).AddTicks(4403),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 7, 25, 11, 39, 45, 390, DateTimeKind.Local).AddTicks(5592));
        }
    }
}
