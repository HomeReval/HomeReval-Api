using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace API.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }
    }
}
