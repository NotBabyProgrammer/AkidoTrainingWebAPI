using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AkidoTrainingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Belt", "ImagePath", "Level" },
                values: new object[] { "Black", "Default.jpg", 1 });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Belt", "ImagePath", "Level" },
                values: new object[] { "Black", "Default.jpg", 2 });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Belt", "ImagePath", "Level" },
                values: new object[] { "Black", "Default.jpg", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Belt", "ImagePath", "Level" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Belt", "ImagePath", "Level" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Belt", "ImagePath", "Level" },
                values: new object[] { null, null, null });
        }
    }
}
