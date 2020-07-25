using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorCentral.Infrastructure.Migrations
{
    public partial class AddUserColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "dbo",
                table: "users",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                schema: "dbo",
                table: "users",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                schema: "dbo",
                table: "users",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                schema: "dbo",
                table: "users",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "first_name",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_name",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password",
                schema: "dbo",
                table: "users");
        }
    }
}
