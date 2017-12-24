using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolSystemWithCore.Data.Migrations
{
    public partial class CreatePrincipalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Principals",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    ApplicationUserId1 = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Principals", x => x.ApplicationUserId);
                    table.ForeignKey(
                        name: "FK_Principals_AspNetUsers_ApplicationUserId1",
                        column: x => x.ApplicationUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Principals_ApplicationUserId1",
                table: "Principals",
                column: "ApplicationUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Principals");
        }
    }
}
