using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShoppingAPI.Migrations
{
    public partial class addedFilePathToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Products");
        }
    }
}
