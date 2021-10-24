using Microsoft.EntityFrameworkCore.Migrations;

namespace StackUnderflow.Data.Migrations
{
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class _0008_RemovedVotesSum : Migration
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1300 // Element should begin with upper-case letter
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
