using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceAPI.Persistence.Migrations
{
    public partial class Migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirationTime",
                table: "AspNetUsers",
                newName: "RefreshTokenExpirationTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpirationTime",
                table: "AspNetUsers",
                newName: "ExpirationTime");
        }
    }
}
