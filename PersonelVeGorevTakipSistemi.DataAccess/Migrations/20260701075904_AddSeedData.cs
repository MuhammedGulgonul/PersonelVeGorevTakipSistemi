using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonelVeGorevTakipSistemi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Şirketin idari ve genel yönetim işleri.", "Yönetim" },
                    { 2, "Yazılım projeleri geliştirme ve Ar-Ge süreçleri.", "Yazılım Geliştirme" },
                    { 3, "İşe alım, eğitim ve çalışan ilişkileri süreçleri.", "İnsan Kaynakları" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedDate", "DepartmentId", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role" },
                values: new object[] { 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "admin@sirket.com", "Ahmet", true, "Yılmaz", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
