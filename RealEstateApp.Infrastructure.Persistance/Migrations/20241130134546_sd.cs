using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class sd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Improvements_Properties_PropertyId",
                table: "Improvements");

            migrationBuilder.DropIndex(
                name: "IX_Improvements_PropertyId",
                table: "Improvements");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Improvements");

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImprovementProperty",
                columns: table => new
                {
                    ImprovementsId = table.Column<int>(type: "int", nullable: false),
                    PropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImprovementProperty", x => new { x.ImprovementsId, x.PropertiesId });
                    table.ForeignKey(
                        name: "FK_ImprovementProperty_Improvements_ImprovementsId",
                        column: x => x.ImprovementsId,
                        principalTable: "Improvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImprovementProperty_Properties_PropertiesId",
                        column: x => x.PropertiesId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PropertyId",
                table: "Favorites",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ImprovementProperty_PropertiesId",
                table: "ImprovementProperty",
                column: "PropertiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "ImprovementProperty");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Improvements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Improvements_PropertyId",
                table: "Improvements",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Improvements_Properties_PropertyId",
                table: "Improvements",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }
    }
}
