using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JamyShell.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "warnings",
                columns: table => new
                {
                    WarnId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorId = table.Column<ulong>(nullable: false),
                    VictimeId = table.Column<ulong>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warnings", x => x.WarnId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "warnings");
        }
    }
}
