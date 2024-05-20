using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class Upd_Change_of_API_User_Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiUsers",
                table: "ApiUsers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ApiUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiUsers",
                table: "ApiUsers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiUsers",
                table: "ApiUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApiUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiUsers",
                table: "ApiUsers",
                column: "Username");
        }
    }
}
