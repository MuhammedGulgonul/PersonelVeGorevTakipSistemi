using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelVeGorevTakipSistemi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleColumnToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Role", "Title" },
                values: new object[] { "Yönetici", "Genel Müdür" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "Admin");
        }
    }
}
