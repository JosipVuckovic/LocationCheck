using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_RequestResponseLog_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestResponseLogs",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiUserEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponseLogs", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_RequestResponseLogs_ApiUsers_ApiUserEntityId",
                        column: x => x.ApiUserEntityId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestResponseLogs_ApiUserEntityId",
                table: "RequestResponseLogs",
                column: "ApiUserEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestResponseLogs");
        }
    }
}
