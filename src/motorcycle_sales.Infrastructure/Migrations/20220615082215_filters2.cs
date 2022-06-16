using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motorcycle_sales.Infrastructure.Migrations
{
    public partial class filters2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilterName",
                table: "UserSearchFilter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterName",
                table: "UserSearchFilter");
        }
    }
}
