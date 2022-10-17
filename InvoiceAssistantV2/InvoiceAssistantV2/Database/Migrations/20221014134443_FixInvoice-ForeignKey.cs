using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class FixInvoiceForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CompanyAddress_AddressToMakeInvoiceOutToId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_PaymentTypes_PaymentTypeID",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentTypeID",
                table: "Invoices",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "AddressToMakeInvoiceOutToId",
                table: "Invoices",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CompanyAddress_AddressToMakeInvoiceOutToId",
                table: "Invoices",
                column: "AddressToMakeInvoiceOutToId",
                principalTable: "CompanyAddress",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_PaymentTypes_PaymentTypeID",
                table: "Invoices",
                column: "PaymentTypeID",
                principalTable: "PaymentTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CompanyAddress_AddressToMakeInvoiceOutToId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_PaymentTypes_PaymentTypeID",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentTypeID",
                table: "Invoices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressToMakeInvoiceOutToId",
                table: "Invoices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CompanyAddress_AddressToMakeInvoiceOutToId",
                table: "Invoices",
                column: "AddressToMakeInvoiceOutToId",
                principalTable: "CompanyAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_PaymentTypes_PaymentTypeID",
                table: "Invoices",
                column: "PaymentTypeID",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
