using Microsoft.EntityFrameworkCore.Migrations;

namespace cvDigiCore.Migrations
{
    public partial class restructuredata5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Category_CategoryId",
                table: "Bridges");

            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Project_ProjectId",
                table: "Bridges");

            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Tags_TagID",
                table: "Bridges");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges");

            migrationBuilder.DropIndex(
                name: "IX_Bridges_CategoryId",
                table: "Bridges");

            migrationBuilder.DropIndex(
                name: "IX_Bridges_TagID",
                table: "Bridges");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Bridges");

            migrationBuilder.DropColumn(
                name: "TagID",
                table: "Bridges");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Category",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Bridges",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Bridges",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges",
                columns: new[] { "CategoryId", "ProjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Category_CategoryId",
                table: "Bridges",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Project_ProjectId",
                table: "Bridges",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Category_CategoryId",
                table: "Bridges");

            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Project_ProjectId",
                table: "Bridges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Category",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Bridges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Bridges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Bridges",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TagID",
                table: "Bridges",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bridges",
                table: "Bridges",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bridges_CategoryId",
                table: "Bridges",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bridges_TagID",
                table: "Bridges",
                column: "TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Category_CategoryId",
                table: "Bridges",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Project_ProjectId",
                table: "Bridges",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Tags_TagID",
                table: "Bridges",
                column: "TagID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
