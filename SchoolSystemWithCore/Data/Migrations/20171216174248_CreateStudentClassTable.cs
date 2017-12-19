using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolSystemWithCore.Data.Migrations
{
    public partial class CreateStudentClassTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    ClassDetailsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_StudentClasses_ClassDetailses_ClassDetailsId",
                        column: x => x.ClassDetailsId,
                        principalTable: "ClassDetailses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentClasses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_ClassDetailsId",
                table: "StudentClasses",
                column: "ClassDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentClasses");
        }
    }
}
