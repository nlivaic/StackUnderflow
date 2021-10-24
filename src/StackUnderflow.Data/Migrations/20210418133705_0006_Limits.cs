using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StackUnderflow.Data.Migrations
{
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1601 // Partial elements should be documented
    public partial class _0006_Limits : Migration
#pragma warning restore SA1601 // Partial elements should be documented
#pragma warning restore SA1300 // Element should begin with upper-case letter
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LimitsKeyValuePairs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LimitKey = table.Column<string>(nullable: true),
                    LimitValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimitsKeyValuePairs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LimitsKeyValuePairs");
        }
    }
}
