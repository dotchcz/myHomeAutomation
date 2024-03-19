using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyHomeAutomation.WebApi.Migrations
{
    public partial class IsExtendingButton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExtendingButton",
                table: "Relays",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExtendingButton",
                table: "Relays");
        }
    }
}
