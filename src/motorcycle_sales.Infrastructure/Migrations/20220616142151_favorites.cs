using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motorcycle_sales.Infrastructure.Migrations
{
    public partial class favorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertisementApplicationUser",
                columns: table => new
                {
                    FavoriteAdvertisementsId = table.Column<int>(type: "int", nullable: false),
                    FavoritedFromUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementApplicationUser", x => new { x.FavoriteAdvertisementsId, x.FavoritedFromUsersId });
                    table.ForeignKey(
                        name: "FK_AdvertisementApplicationUser_Advertisements_FavoriteAdvertisementsId",
                        column: x => x.FavoriteAdvertisementsId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementApplicationUser_AspNetUsers_FavoritedFromUsersId",
                        column: x => x.FavoritedFromUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementApplicationUser_FavoritedFromUsersId",
                table: "AdvertisementApplicationUser",
                column: "FavoritedFromUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementApplicationUser");
        }
    }
}
