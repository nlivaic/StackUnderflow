using Microsoft.EntityFrameworkCore.Migrations;

namespace StackUnderflow.Data.Migrations
{
    public partial class _0008_RemovedVotesSum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotesSum",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "VotesSum",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "VotesSum",
                table: "Answers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VotesSum",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VotesSum",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VotesSum",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
