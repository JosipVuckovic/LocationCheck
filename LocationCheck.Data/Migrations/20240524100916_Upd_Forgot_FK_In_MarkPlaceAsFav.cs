using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class Upd_Forgot_FK_In_MarkPlaceAsFav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaceExternalIdentifier",
                table: "FavoritePlaces");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaceExternalIdentifier",
                table: "FavoritePlaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
