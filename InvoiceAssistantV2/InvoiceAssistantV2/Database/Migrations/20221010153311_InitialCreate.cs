using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FriendlyName = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    HasBeenDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyDetailsID = table.Column<int>(type: "INTEGER", nullable: false),
                    FriendlyName = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressLine2 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressLine3 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressLine4 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressLine5 = table.Column<string>(type: "TEXT", nullable: true),
                    PostCode = table.Column<string>(type: "TEXT", nullable: true),
                    DrivingDistanceToAddress = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAddress_CompanyDetails_CompanyDetailsID",
                        column: x => x.CompanyDetailsID,
                        principalTable: "CompanyDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfInvoice = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateRecievedMoney = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TotalInvoiceAmmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    AddressToMakeInvoiceOutToId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_CompanyAddress_AddressToMakeInvoiceOutToId",
                        column: x => x.AddressToMakeInvoiceOutToId,
                        principalTable: "CompanyAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoicesPaymentBreakDown",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Ammount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicesPaymentBreakDown", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoicesPaymentBreakDown_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlacesVisitedForInvoice",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfTimesVisited = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacesVisitedForInvoice", x => new { x.InvoiceId, x.CompanyAddressId });
                    table.ForeignKey(
                        name: "FK_PlacesVisitedForInvoice_CompanyAddress_CompanyAddressId",
                        column: x => x.CompanyAddressId,
                        principalTable: "CompanyAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlacesVisitedForInvoice_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAddress_CompanyDetailsID",
                table: "CompanyAddress",
                column: "CompanyDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AddressToMakeInvoiceOutToId",
                table: "Invoices",
                column: "AddressToMakeInvoiceOutToId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaymentTypeId",
                table: "Invoices",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesPaymentBreakDown_InvoiceId",
                table: "InvoicesPaymentBreakDown",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacesVisitedForInvoice_CompanyAddressId",
                table: "PlacesVisitedForInvoice",
                column: "CompanyAddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoicesPaymentBreakDown");

            migrationBuilder.DropTable(
                name: "PlacesVisitedForInvoice");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "CompanyAddress");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "CompanyDetails");
        }
    }
}
