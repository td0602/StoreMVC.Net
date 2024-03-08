using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store_App.Migrations
{
    public partial class updatenullforNoteofOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Order",
                newName: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Order",
                newName: "UserName");
        }
    }
}
