using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_ApiUsers_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApiKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.Username);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiUsers_ApiKey",
                table: "ApiUsers",
                column: "ApiKey");

            migrationBuilder.CreateIndex(
                name: "IX_ApiUsers_Username",
                table: "ApiUsers",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiUsers");
        }
    }
}
