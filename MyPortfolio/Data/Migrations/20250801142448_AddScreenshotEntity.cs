﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPortfolio.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScreenshotEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScreenShots",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "Screenshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screenshots_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_ProjectId",
                table: "Screenshots",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.AddColumn<string>(
                name: "ScreenShots",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
