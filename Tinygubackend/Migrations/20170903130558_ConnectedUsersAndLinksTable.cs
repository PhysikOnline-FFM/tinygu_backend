using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tinygubackend.Migrations
{
    public partial class ConnectedUsersAndLinksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Links",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_OwnerId",
                table: "Links",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Users_OwnerId",
                table: "Links",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Users_OwnerId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_OwnerId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Links");
        }
    }
}
