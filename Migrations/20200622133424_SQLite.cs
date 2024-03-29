﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace UrunListem.Migrations
{
    public partial class SQLite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Adi = table.Column<string>(maxLength: 50, nullable: false),
                    Tur = table.Column<string>(nullable: true),
                    Renk = table.Column<string>(nullable: true),
                    Aciklama = table.Column<string>(nullable: true),
                    Fiyat = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resimler",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResimAdi = table.Column<string>(nullable: true),
                    UrunuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resimler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resimler_Urunler_UrunuId",
                        column: x => x.UrunuId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resimler_UrunuId",
                table: "Resimler",
                column: "UrunuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resimler");

            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
