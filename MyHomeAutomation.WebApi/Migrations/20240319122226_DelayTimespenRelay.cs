using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyHomeAutomation.WebApi.Migrations
{
    public partial class DelayTimespenRelay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Delay",
                table: "Relays",
                type: "interval",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delay",
                table: "Relays");
        }
    }
}
