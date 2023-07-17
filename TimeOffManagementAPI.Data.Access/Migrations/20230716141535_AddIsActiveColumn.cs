using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeOffManagementAPI.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffRequests_AspNetUsers_userId",
                table: "TimeOffRequests");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "TimeOffRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffRequests_AspNetUsers_userId",
                table: "TimeOffRequests",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffRequests_AspNetUsers_userId",
                table: "TimeOffRequests");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "TimeOffRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffRequests_AspNetUsers_userId",
                table: "TimeOffRequests",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
