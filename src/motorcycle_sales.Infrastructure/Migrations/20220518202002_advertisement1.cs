using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motorcycle_sales.Infrastructure.Migrations
{
    public partial class advertisement1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Advertisements");
        }
    }
}
