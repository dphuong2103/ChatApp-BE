using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEnd.Migrations
{
    public partial class updateFileStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileStatus",
                table: "Messages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileStatus",
                table: "Messages");
        }
    }
}
