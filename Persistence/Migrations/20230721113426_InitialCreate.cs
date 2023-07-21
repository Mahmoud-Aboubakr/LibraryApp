using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AuthorPhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    AuthorProfits = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
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
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CustomerPhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmpType = table.Column<byte>(type: "tinyint", nullable: false),
                    EmpAge = table.Column<int>(type: "int", nullable: false),
                    EmpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpPhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    EmpStartingShift = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpEndingShift = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpBasicSalary = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpId);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublisherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PublisherPhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PublisherId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(4477)),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "ReturnedOrders",
                columns: table => new
                {
                    ReturnedOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginOrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 558, DateTimeKind.Local).AddTicks(131)),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedOrders", x => x.ReturnedOrderId);
                    table.ForeignKey(
                        name: "FK_ReturnedOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
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
                    Permission = table.Column<int>(type: "int", nullable: false),
                    DayDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 556, DateTimeKind.Local).AddTicks(5787)),
                    Month = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendence_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId");
                });

            migrationBuilder.CreateTable(
                name: "BannedCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    BanDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 556, DateTimeKind.Local).AddTicks(8825)),
                    EmpId = table.Column<int>(type: "int", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_BannedCustomers_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId");
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(9248)),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
                    Bonus = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
                    Deduct = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
                    TotalSalary = table.Column<decimal>(type: "decimal(18,3)", nullable: false, computedColumnSql: "[BasicSalary] + [Bonus] - [Deduct]")
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
                    DayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NormalVacation = table.Column<bool>(type: "bit", nullable: true),
                    UrgentVacation = table.Column<bool>(type: "bit", nullable: true),
                    Absence = table.Column<bool>(type: "bit", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false)
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
                name: "BookOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookOrderDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookOrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    BorrowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 21, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(2131)),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 7, 24, 14, 34, 26, 557, DateTimeKind.Local).AddTicks(2381))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.BorrowId);
                    table.ForeignKey(
                        name: "FK_Borrows_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId");
                    table.ForeignKey(
                        name: "FK_Borrows_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedOrderId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnOrderDetails_ReturnedOrders_ReturnedOrderId",
                        column: x => x.ReturnedOrderId,
                        principalTable: "ReturnedOrders",
                        principalColumn: "ReturnedOrderId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_BookOrderDetails_BookId",
                table: "BookOrderDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrderDetails_OrderId",
                table: "BookOrderDetails",
                column: "OrderId");

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
                name: "IX_Borrows_CustomerId",
                table: "Borrows",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_EmpId",
                table: "Payrolls",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedOrders_CustomerId",
                table: "ReturnedOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderDetails_BookId",
                table: "ReturnOrderDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderDetails_ReturnedOrderId",
                table: "ReturnOrderDetails",
                column: "ReturnedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_EmpId",
                table: "Vacations",
                column: "EmpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendence");

            migrationBuilder.DropTable(
                name: "BannedCustomers");

            migrationBuilder.DropTable(
                name: "BookOrderDetails");

            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "ReturnOrderDetails");

            migrationBuilder.DropTable(
                name: "Vacations");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "ReturnedOrders");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
