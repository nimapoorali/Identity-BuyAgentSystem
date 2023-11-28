using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Persistence.Migrations
{
    public partial class resolvedEmailMobile2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mobile_Value",
                table: "Users",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "Mobile_IsVerified",
                table: "Users",
                newName: "MobileIsVerified");

            migrationBuilder.RenameColumn(
                name: "Email_Value",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Email_IsVerified",
                table: "Users",
                newName: "EmailIsVerified");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileIsVerified",
                table: "Users",
                newName: "Mobile_IsVerified");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Users",
                newName: "Mobile_Value");

            migrationBuilder.RenameColumn(
                name: "EmailIsVerified",
                table: "Users",
                newName: "Email_IsVerified");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Email_Value");
        }
    }
}
