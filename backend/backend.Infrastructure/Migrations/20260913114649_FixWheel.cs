using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixWheel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "wheel_items");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "wheel_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "wheel_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "wheel_items");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "wheel_items");

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "wheel_items",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
