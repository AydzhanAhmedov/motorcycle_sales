using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motorcycle_sales.Infrastructure.Migrations
{
    public partial class advertisement5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Advertisements");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "AdvertisementSearchFilter",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "AdvertisementSearchFilter");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Advertisements");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
