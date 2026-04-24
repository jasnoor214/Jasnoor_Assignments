using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_in_ASPNET_MVC.Migrations
{
    /// <inheritdoc />
    public partial class imageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "employees");
        }
    }
}
