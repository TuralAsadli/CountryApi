using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryInfoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceImg_RecomendedPlace_RecomendedPlaceId",
                table: "PlaceImg");

            migrationBuilder.DropForeignKey(
                name: "FK_RecomendedPlace_Cities_CityId",
                table: "RecomendedPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecomendedPlace",
                table: "RecomendedPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaceImg",
                table: "PlaceImg");

            migrationBuilder.RenameTable(
                name: "RecomendedPlace",
                newName: "RecomendedPlaces");

            migrationBuilder.RenameTable(
                name: "PlaceImg",
                newName: "PlaceImgs");

            migrationBuilder.RenameIndex(
                name: "IX_RecomendedPlace_CityId",
                table: "RecomendedPlaces",
                newName: "IX_RecomendedPlaces_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceImg_RecomendedPlaceId",
                table: "PlaceImgs",
                newName: "IX_PlaceImgs_RecomendedPlaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecomendedPlaces",
                table: "RecomendedPlaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaceImgs",
                table: "PlaceImgs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CityImgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityImgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityImgs_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityImgs_CityId",
                table: "CityImgs",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceImgs_RecomendedPlaces_RecomendedPlaceId",
                table: "PlaceImgs",
                column: "RecomendedPlaceId",
                principalTable: "RecomendedPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecomendedPlaces_Cities_CityId",
                table: "RecomendedPlaces",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceImgs_RecomendedPlaces_RecomendedPlaceId",
                table: "PlaceImgs");

            migrationBuilder.DropForeignKey(
                name: "FK_RecomendedPlaces_Cities_CityId",
                table: "RecomendedPlaces");

            migrationBuilder.DropTable(
                name: "CityImgs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecomendedPlaces",
                table: "RecomendedPlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaceImgs",
                table: "PlaceImgs");

            migrationBuilder.RenameTable(
                name: "RecomendedPlaces",
                newName: "RecomendedPlace");

            migrationBuilder.RenameTable(
                name: "PlaceImgs",
                newName: "PlaceImg");

            migrationBuilder.RenameIndex(
                name: "IX_RecomendedPlaces_CityId",
                table: "RecomendedPlace",
                newName: "IX_RecomendedPlace_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceImgs_RecomendedPlaceId",
                table: "PlaceImg",
                newName: "IX_PlaceImg_RecomendedPlaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecomendedPlace",
                table: "RecomendedPlace",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaceImg",
                table: "PlaceImg",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceImg_RecomendedPlace_RecomendedPlaceId",
                table: "PlaceImg",
                column: "RecomendedPlaceId",
                principalTable: "RecomendedPlace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecomendedPlace_Cities_CityId",
                table: "RecomendedPlace",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
