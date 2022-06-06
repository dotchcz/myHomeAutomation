using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyHomeAutomation.WebApi.Migrations
{
    public partial class RelayIpAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Relays",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Relays");
        }
    }
}
