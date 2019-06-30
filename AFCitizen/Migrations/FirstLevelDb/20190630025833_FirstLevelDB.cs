using Microsoft.EntityFrameworkCore.Migrations;

namespace AFCitizen.Migrations.FirstLevelDb
{
    public partial class FirstLevelDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DocId = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<string>(nullable: true),
                    isClosed = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    AuthorityType = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Document = table.Column<string>(nullable: true),
                    Replies = table.Column<string>(nullable: true),
                    TypeMessage = table.Column<string>(nullable: true),
                    PreviousHash = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
