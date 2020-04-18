using Microsoft.EntityFrameworkCore.Migrations;

namespace cvDigiCore.Migrations
{
    public partial class restructuredata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Bridges",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Bridges_CategoryId",
                table: "Bridges",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges");

            migrationBuilder.DropIndex(
                name: "IX_Bridges_CategoryId",
                table: "Bridges");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Bridges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges",
                columns: new[] { "CategoryId", "TagId", "ProjectId" });
        }
    }
}
