using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeOffManagementAPI.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class HolidayTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayNameTr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HolidayNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DayOfWeekTr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LongDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holidays");
        }
    }
}
