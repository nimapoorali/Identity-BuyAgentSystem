using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Persistence.Migrations
{
    public partial class RemovedActivityStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_ActivityStates_ActivityStateValue",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_ActivityStateValue",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "ActivityStateValue",
                table: "UserRoles",
                newName: "ActivityState");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivityState",
                table: "UserRoles",
                newName: "ActivityStateValue");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ActivityStateValue",
                table: "UserRoles",
                column: "ActivityStateValue");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_ActivityStates_ActivityStateValue",
                table: "UserRoles",
                column: "ActivityStateValue",
                principalTable: "ActivityStates",
                principalColumn: "Value",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
