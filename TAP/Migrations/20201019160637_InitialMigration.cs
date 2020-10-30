using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAP.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.id);
                    table.ForeignKey(
                        name: "project_client_fk",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__client__3213E83E6490E155",
                table: "client",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__project__BF21A4252F6E877C",
                table: "project",
                column: "client_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__project__3213E83E6A3395D8",
                table: "project",
                column: "id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "client");
        }
    }
}
