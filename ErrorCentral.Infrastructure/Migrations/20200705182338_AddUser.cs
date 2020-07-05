using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorCentral.Infrastructure.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "dbo",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Password",
                schema: "dbo",
                table: "users");
        }
    }
}
