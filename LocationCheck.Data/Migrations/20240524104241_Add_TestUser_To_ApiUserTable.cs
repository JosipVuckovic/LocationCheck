using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_TestUser_To_ApiUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO ApiUsers(Username, ApiKey, Active) Values ('test', 'c740526d-06b4-4a0b-8a57-6f37404e4a6d', 'true')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
