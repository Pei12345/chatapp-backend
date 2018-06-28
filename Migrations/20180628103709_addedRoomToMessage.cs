using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.API.Migrations
{
    public partial class addedRoomToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "ChatMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Room",
                table: "ChatMessages");
        }
    }
}
