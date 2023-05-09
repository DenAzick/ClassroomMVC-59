using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomData.Migrations
{
    /// <inheritdoc />
    public partial class InitDb12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSciences",
                table: "UserSciences");

            migrationBuilder.DropColumn(
                name: "ScinceId",
                table: "UserSciences");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSciences",
                table: "UserSciences",
                columns: new[] { "UserId", "ScienceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSciences",
                table: "UserSciences");

            migrationBuilder.AddColumn<Guid>(
                name: "ScinceId",
                table: "UserSciences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSciences",
                table: "UserSciences",
                columns: new[] { "UserId", "ScinceId" });
        }
    }
}
