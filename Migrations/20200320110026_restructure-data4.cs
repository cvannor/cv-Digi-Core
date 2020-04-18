using Microsoft.EntityFrameworkCore.Migrations;

namespace cvDigiCore.Migrations
{
    public partial class restructuredata4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Tags_TagId",
                table: "Bridges");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Bridges",
                newName: "TagID");

            migrationBuilder.RenameIndex(
                name: "IX_Bridges_TagId",
                table: "Bridges",
                newName: "IX_Bridges_TagID");

            migrationBuilder.AddColumn<int>(
                name: "CatId",
                table: "Tags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Category",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Bridges",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Tags_TagID",
                table: "Bridges",
                column: "TagID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Tags_TagID",
                table: "Bridges");

            migrationBuilder.DropColumn(
                name: "CatId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Bridges");

            migrationBuilder.RenameColumn(
                name: "TagID",
                table: "Bridges",
                newName: "TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Bridges_TagID",
                table: "Bridges",
                newName: "IX_Bridges_TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Tags_TagId",
                table: "Bridges",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
