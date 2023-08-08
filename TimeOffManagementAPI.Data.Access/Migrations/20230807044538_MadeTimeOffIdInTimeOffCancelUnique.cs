using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeOffManagementAPI.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class MadeTimeOffIdInTimeOffCancelUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TimeOffCancels_TimeOffId",
                table: "TimeOffCancels",
                column: "TimeOffId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeOffCancels_TimeOffId",
                table: "TimeOffCancels");
        }
    }
}
