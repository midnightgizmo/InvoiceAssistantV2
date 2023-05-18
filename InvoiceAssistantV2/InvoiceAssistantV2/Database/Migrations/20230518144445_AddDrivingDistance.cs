using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class AddDrivingDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "DrivingDistanceToAddress",
                table: "PlacesVisitedForInvoice",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "DrivingDistanceToAddress",
                table: "PlacesVisitedForInvoice");

            
        }
    }
}
