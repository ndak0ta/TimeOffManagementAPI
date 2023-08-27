using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeOffManagementAPI.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusToTimeOff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCancelRequest",
                table: "TimeOffs");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "TimeOffs");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "TimeOffs");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "TimeOffs");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "AspNetUsers",
                newName: "IsActive");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TimeOffs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TimeOffs");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AspNetUsers",
                newName: "isActive");

            migrationBuilder.AddColumn<bool>(
                name: "HasCancelRequest",
                table: "TimeOffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "TimeOffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "TimeOffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "TimeOffs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
