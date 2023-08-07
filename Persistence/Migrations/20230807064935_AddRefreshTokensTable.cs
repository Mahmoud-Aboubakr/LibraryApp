using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddRefreshTokensTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 630, DateTimeKind.Local).AddTicks(3171),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 481, DateTimeKind.Local).AddTicks(6528));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 629, DateTimeKind.Local).AddTicks(7848),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 481, DateTimeKind.Local).AddTicks(3693));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 629, DateTimeKind.Local).AddTicks(6758),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 481, DateTimeKind.Local).AddTicks(2740));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 629, DateTimeKind.Local).AddTicks(2787),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(9730));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 10, 9, 49, 35, 628, DateTimeKind.Local).AddTicks(9145),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 9, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(7514));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 628, DateTimeKind.Local).AddTicks(8844),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(7286));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(8740),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(4033));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpLeavingTime",
                table: "Attendence",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(1016),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(899));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpArrivalTime",
                table: "Attendence",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(427),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(629));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(1502),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(1040));

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.ApplicationUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Vacations",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 481, DateTimeKind.Local).AddTicks(6528),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 630, DateTimeKind.Local).AddTicks(3171));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ReturnedOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 481, DateTimeKind.Local).AddTicks(3693),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 629, DateTimeKind.Local).AddTicks(7848));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SalaryDate",
                table: "Payrolls",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 481, DateTimeKind.Local).AddTicks(2740),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 629, DateTimeKind.Local).AddTicks(6758));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(9730),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 629, DateTimeKind.Local).AddTicks(2787));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 9, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(7514),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 10, 9, 49, 35, 628, DateTimeKind.Local).AddTicks(9145));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BorrowDate",
                table: "Borrows",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(7286),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 628, DateTimeKind.Local).AddTicks(8844));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanDate",
                table: "BannedCustomers",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(4033),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(8740));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpLeavingTime",
                table: "Attendence",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(899),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(1016));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmpArrivalTime",
                table: "Attendence",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(629),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(427));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DayDate",
                table: "Attendence",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 13, 4, 19, 480, DateTimeKind.Local).AddTicks(1040),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 7, 9, 49, 35, 627, DateTimeKind.Local).AddTicks(1502));
        }
    }
}
