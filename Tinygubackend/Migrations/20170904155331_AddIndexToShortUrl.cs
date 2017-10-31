using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tinygubackend.Migrations
{
    public partial class AddIndexToShortUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortUrl",
                table: "Links",
                maxLength: 95,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_ShortUrl",
                table: "Links",
                column: "ShortUrl",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Links_ShortUrl",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "ShortUrl",
                table: "Links",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 95,
                oldNullable: true);
        }
    }
}
