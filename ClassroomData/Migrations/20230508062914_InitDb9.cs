using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomData.Migrations
{
    /// <inheritdoc />
    public partial class InitDb9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sciences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sciences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sciences_Sciences_ScienceId",
                        column: x => x.ScienceId,
                        principalTable: "Sciences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sciences_schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSciences",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSciences", x => new { x.UserId, x.ScinceId });
                    table.ForeignKey(
                        name: "FK_UserSciences_Sciences_ScienceId",
                        column: x => x.ScienceId,
                        principalTable: "Sciences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSciences_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sciences_SchoolId",
                table: "Sciences",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Sciences_ScienceId",
                table: "Sciences",
                column: "ScienceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSciences_ScienceId",
                table: "UserSciences",
                column: "ScienceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSciences");

            migrationBuilder.DropTable(
                name: "Sciences");
        }
    }
}
