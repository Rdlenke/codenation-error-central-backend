using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorCentral.Infrastructure.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                schema: "dbo",
                table: "users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "dbo",
                table: "users",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "dbo",
                table: "users",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "dbo",
                table: "users",
                newName: "email");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                schema: "dbo",
                table: "users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "lastName",
                schema: "dbo",
                table: "users",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "firstName",
                schema: "dbo",
                table: "users",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "dbo",
                table: "users",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                schema: "dbo",
                table: "users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "lastName",
                schema: "dbo",
                table: "users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstName",
                schema: "dbo",
                table: "users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "dbo",
                table: "users",
                newName: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "dbo",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);
        }
    }
}
