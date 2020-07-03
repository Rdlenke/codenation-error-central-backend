using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorCentral.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    removed = table.Column<bool>(nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(nullable: false),
                    updated_at = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_errors",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    removed = table.Column<bool>(nullable: false, defaultValue: false),
                    id_user = table.Column<int>(nullable: false),
                    title = table.Column<string>(maxLength: 500, nullable: false),
                    details = table.Column<string>(maxLength: 2000, nullable: true),
                    source = table.Column<string>(maxLength: 300, nullable: false),
                    filed = table.Column<bool>(nullable: false, defaultValue: false),
                    e_level = table.Column<int>(nullable: false),
                    e_environment = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTimeOffset>(nullable: false),
                    updated_at = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_errors", x => x.id);
                    table.ForeignKey(
                        name: "FK_log_errors_users_id_user",
                        column: x => x.id_user,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_log_errors_id_user",
                schema: "dbo",
                table: "log_errors",
                column: "id_user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_errors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");
        }
    }
}
