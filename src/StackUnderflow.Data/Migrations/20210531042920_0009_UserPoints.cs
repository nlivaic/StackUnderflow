using Microsoft.EntityFrameworkCore.Migrations;

namespace StackUnderflow.Data.Migrations
{
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class _0009_UserPoints : Migration
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1300 // Element should begin with upper-case letter
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Users");
        }
    }
}
