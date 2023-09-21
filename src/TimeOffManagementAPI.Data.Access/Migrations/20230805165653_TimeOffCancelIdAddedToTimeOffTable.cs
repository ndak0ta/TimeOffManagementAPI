using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TimeOffManagementAPI.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class TimeOffCancelIdAddedToTimeOffTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dc5534b4-76b3-47bd-b8b6-cb86a2b16759", "4a2f102f-517c-417c-a2b5-6269ac813f3f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e146986f-0e75-4900-b33a-1e9d55cc922f", "581d668b-ef06-45e5-aaa0-e96a2e794186" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc5534b4-76b3-47bd-b8b6-cb86a2b16759");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e146986f-0e75-4900-b33a-1e9d55cc922f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4a2f102f-517c-417c-a2b5-6269ac813f3f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "581d668b-ef06-45e5-aaa0-e96a2e794186");

            migrationBuilder.AddColumn<int>(
                name: "TimeOffCancelId",
                table: "TimeOffs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOffCancelId",
                table: "TimeOffs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsActive", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dc5534b4-76b3-47bd-b8b6-cb86a2b16759", "5ee9fe06-e0ec-43bd-b5a8-9d27e8489bab", true, "Manager", "MANAGER" },
                    { "e146986f-0e75-4900-b33a-1e9d55cc922f", "b5778eff-061d-45bc-8d70-40812b458826", true, "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AnnualTimeOffs", "AutomaticAnnualTimeOffIncrement", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "HireDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RemainingAnnualTimeOffs", "SecurityStamp", "TwoFactorEnabled", "UserName", "isActive" },
                values: new object[,]
                {
                    { "4a2f102f-517c-417c-a2b5-6269ac813f3f", 0, "Admin Address", 0, false, "292d5678-3373-45e9-85da-3426a08a41c7", new DateTime(2023, 8, 4, 14, 46, 20, 406, DateTimeKind.Local).AddTicks(10), "", false, "Admin", new DateTime(2023, 8, 4, 14, 46, 20, 406, DateTimeKind.Local).AddTicks(50), "Admin", false, null, null, null, "AQAAAAEAACcQAAAAEOYCPN/vmUxX72ttUwkAaBcxB55FvsixXBdE2T/+gqKPAfCWZuea8Ri3jSor5lZRuA==", "0000000000", false, 0, "d738fc69-6a5b-4522-8e05-e2b718368159", false, "admin", true },
                    { "581d668b-ef06-45e5-aaa0-e96a2e794186", 0, "Employee Address", 0, false, "134fdd6e-5835-4ef2-8706-f6cd360e292f", new DateTime(2023, 8, 4, 14, 46, 20, 406, DateTimeKind.Local).AddTicks(80), "", false, "Employee", new DateTime(2023, 8, 4, 14, 46, 20, 406, DateTimeKind.Local).AddTicks(80), "Employee", false, null, null, null, "AQAAAAEAACcQAAAAEP1xrpQProqtCI3G30xCcsFQ1fpcBzTXWdzMn8yFkG5Wa0HlrRr4Pg+fOHLCv1sFGQ==", "0000000000", false, 0, "f5b8d224-543d-4612-9631-c5894e7e418f", false, "employee", true }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "dc5534b4-76b3-47bd-b8b6-cb86a2b16759", "4a2f102f-517c-417c-a2b5-6269ac813f3f" },
                    { "e146986f-0e75-4900-b33a-1e9d55cc922f", "581d668b-ef06-45e5-aaa0-e96a2e794186" }
                });
        }
    }
}
