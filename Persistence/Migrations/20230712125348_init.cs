using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorProfits = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublisherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublisherPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PublisherId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId");
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "PublisherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 12, 15, 53, 48, 95, DateTimeKind.Local).AddTicks(7817)),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Ordertype = table.Column<bool>(type: "bit", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId");
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 12, 15, 53, 48, 95, DateTimeKind.Local).AddTicks(2273)),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 15, 15, 53, 48, 95, DateTimeKind.Local).AddTicks(2921)),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Borrows_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Borrows_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "OrdersBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersBooks_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    EmpArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpLeavingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Permission = table.Column<int>(type: "int", nullable: true),
                    DayDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2023, 7, 12, 15, 53, 48, 93, DateTimeKind.Local).AddTicks(2961)),
                    Month = table.Column<byte>(type: "tinyint", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannedCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    BanDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2023, 7, 12, 15, 53, 48, 94, DateTimeKind.Local).AddTicks(3689)),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannedCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpType = table.Column<byte>(type: "tinyint", nullable: false),
                    EmpAge = table.Column<int>(type: "int", nullable: false),
                    EmpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpStartingShift = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpEndingShift = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpBasicSalary = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    AttendenceId = table.Column<int>(type: "int", nullable: true),
                    PayrollId = table.Column<int>(type: "int", nullable: true),
                    VacationId = table.Column<int>(type: "int", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_Employees_Attendence_AttendenceId",
                        column: x => x.AttendenceId,
                        principalTable: "Attendence",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BasicSalary = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Bonus = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Deduct = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    TotalSalary = table.Column<float>(type: "real", nullable: false, computedColumnSql: "[BasicSalary] + [Bonus] - [Deduct]"),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payrolls_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId");
                });

            migrationBuilder.CreateTable(
                name: "Vacations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    DayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NormalVacation = table.Column<bool>(type: "bit", nullable: true),
                    UrgentVacation = table.Column<bool>(type: "bit", nullable: true),
                    Absence = table.Column<bool>(type: "bit", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacations_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendence_EmpId",
                table: "Attendence",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_BannedCustomers_CustomerId",
                table: "BannedCustomers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BannedCustomers_EmpId",
                table: "BannedCustomers",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_OrderId",
                table: "Borrows",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AttendenceId",
                table: "Employees",
                column: "AttendenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PayrollId",
                table: "Employees",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_VacationId",
                table: "Employees",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BookId",
                table: "Orders",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersBooks_BookId",
                table: "OrdersBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersBooks_OrderId",
                table: "OrdersBooks",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_EmpId",
                table: "Payrolls",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_EmpId",
                table: "Vacations",
                column: "EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendence_Employees_EmpId",
                table: "Attendence",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_BannedCustomers_Employees_EmpId",
                table: "BannedCustomers",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Payrolls_PayrollId",
                table: "Employees",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Vacations_VacationId",
                table: "Employees",
                column: "VacationId",
                principalTable: "Vacations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendence_Employees_EmpId",
                table: "Attendence");

            migrationBuilder.DropForeignKey(
                name: "FK_Payrolls_Employees_EmpId",
                table: "Payrolls");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_EmpId",
                table: "Vacations");

            migrationBuilder.DropTable(
                name: "BannedCustomers");

            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "OrdersBooks");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Attendence");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "Vacations");
        }
    }
}
