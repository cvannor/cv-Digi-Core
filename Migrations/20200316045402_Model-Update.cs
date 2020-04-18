using Microsoft.EntityFrameworkCore.Migrations;

namespace cvDigiCore.Migrations
{
    public partial class ModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Tag_TagID",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Category_CategoryID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_CategoryID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Category_TagID",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TagID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TagID",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Tag",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Category",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(nullable: true),
                    ProjectID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Images_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CategoryID",
                table: "Tag",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ProjectID",
                table: "Category",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProjectID",
                table: "Images",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Project_ProjectID",
                table: "Category",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Category_CategoryID",
                table: "Tag",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Project_ProjectID",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Category_CategoryID",
                table: "Tag");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Tag_CategoryID",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Category_ProjectID",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagID",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagID",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Project_CategoryID",
                table: "Project",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_TagID",
                table: "Category",
                column: "TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Tag_TagID",
                table: "Category",
                column: "TagID",
                principalTable: "Tag",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Category_CategoryID",
                table: "Project",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
