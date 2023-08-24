using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEnd.Migrations
{
    public partial class updateFileUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgUrls",
                table: "Messages",
                newName: "FileUrls");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileUrls",
                table: "Messages",
                newName: "ImgUrls");
        }
    }
}
