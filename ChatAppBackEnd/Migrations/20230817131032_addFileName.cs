using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEnd.Migrations
{
    public partial class addFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Messages");
        }
    }
}
