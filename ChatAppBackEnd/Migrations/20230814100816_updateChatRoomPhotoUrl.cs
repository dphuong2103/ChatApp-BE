using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEnd.Migrations
{
    public partial class updateChatRoomPhotoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "ChatRooms",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotmeoUrl",
                table: "ChatRooms");
        }
    }
}
