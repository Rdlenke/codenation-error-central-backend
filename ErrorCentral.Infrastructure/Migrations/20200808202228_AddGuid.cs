using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorCentral.Infrastructure.Migrations
{
    public partial class AddGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "guid",
                schema: "dbo",
                table: "users",
                nullable: false,
                defaultValue: new Guid("347cf1b7-f83d-4edc-a192-16a4c4be5002"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("c922a8f5-c75e-40c0-bd7f-52e457a6f5da"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "guid",
                schema: "dbo",
                table: "users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("c922a8f5-c75e-40c0-bd7f-52e457a6f5da"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("347cf1b7-f83d-4edc-a192-16a4c4be5002"));
        }
    }
}
