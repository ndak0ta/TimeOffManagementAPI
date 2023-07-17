using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeOffManagementAPI.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNameChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_AspNetUsers_userId",
                table: "TimeOffs");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "TimeOffs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "isApproved",
                table: "TimeOffs",
                newName: "IsApproved");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "TimeOffs",
                newName: "IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_TimeOffs_userId",
                table: "TimeOffs",
                newName: "IX_TimeOffs_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_AspNetUsers_UserId",
                table: "TimeOffs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_AspNetUsers_UserId",
                table: "TimeOffs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TimeOffs",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "TimeOffs",
                newName: "isApproved");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "TimeOffs",
                newName: "isActive");

            migrationBuilder.RenameIndex(
                name: "IX_TimeOffs_UserId",
                table: "TimeOffs",
                newName: "IX_TimeOffs_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_AspNetUsers_userId",
                table: "TimeOffs",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
