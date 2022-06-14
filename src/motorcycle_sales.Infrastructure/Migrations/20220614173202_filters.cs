using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motorcycle_sales.Infrastructure.Migrations
{
    public partial class filters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertisementSearchFilter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: true),
                    EngineType = table.Column<int>(type: "int", nullable: true),
                    TransmissionType = table.Column<int>(type: "int", nullable: true),
                    CoolingSystemType = table.Column<int>(type: "int", nullable: true),
                    PriceFrom = table.Column<double>(type: "float", nullable: true),
                    PriceTo = table.Column<double>(type: "float", nullable: true),
                    ProductionYearFrom = table.Column<int>(type: "int", nullable: true),
                    ProductionYearTo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementSearchFilter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSearchFilter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementSearchFilterId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotificationsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSearchFilter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSearchFilter_AdvertisementSearchFilter_AdvertisementSearchFilterId",
                        column: x => x.AdvertisementSearchFilterId,
                        principalTable: "AdvertisementSearchFilter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchFilter_AdvertisementSearchFilterId",
                table: "UserSearchFilter",
                column: "AdvertisementSearchFilterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSearchFilter");

            migrationBuilder.DropTable(
                name: "AdvertisementSearchFilter");
        }
    }
}
