using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VY.Hackathon.TeamOne.Migrations
{
    public partial class SnapshotRepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultSnapshot",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnapshotDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    SnapshotDataJson = table.Column<string>(type: "varchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultSnapshot", x => x.SnapshotId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultSnapshot");
        }
    }
}
