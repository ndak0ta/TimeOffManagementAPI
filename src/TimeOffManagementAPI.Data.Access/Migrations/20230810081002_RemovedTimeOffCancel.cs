using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeOffManagementAPI.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTimeOffCancel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeOffCancels");

            migrationBuilder.AddColumn<bool>(
                name: "HasCancelRequest",
                table: "TimeOffs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCancelRequest",
                table: "TimeOffs");

            migrationBuilder.CreateTable(
                name: "TimeOffCancels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsPending = table.Column<bool>(type: "bit", nullable: false),
                    TimeOffId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeOffCancels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeOffCancels_TimeOffId",
                table: "TimeOffCancels",
                column: "TimeOffId",
                unique: true);
        }
    }
}
