using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransitLine.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "DriverUserId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverUserId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Orders",
                type: "int",
                nullable: true);
        }
    }
}
