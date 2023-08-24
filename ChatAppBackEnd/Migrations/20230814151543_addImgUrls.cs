using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEnd.Migrations
{
    public partial class addImgUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ImgUrls",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrls",
                table: "Messages");


        }
    }
}
