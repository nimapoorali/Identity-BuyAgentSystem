using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Persistence.Migrations
{
    public partial class resolvedEmailMobile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Mobile_Value");

            migrationBuilder.AddColumn<bool>(
                name: "Email_IsVerified",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email_Value",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Mobile_IsVerified",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email_IsVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Mobile_IsVerified",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Mobile_Value",
                table: "Users",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Users",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);
        }
    }
}
