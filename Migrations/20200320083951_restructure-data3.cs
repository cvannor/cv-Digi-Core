using Microsoft.EntityFrameworkCore.Migrations;

namespace cvDigiCore.Migrations
{
    public partial class restructuredata3 : Migration
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
                name: "FK_Bridges_Tags_TagId",
                table: "Bridges");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "Bridges",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Bridges",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Bridges",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Bridges_Tags_TagId",
                table: "Bridges",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Category_CategoryId",
                table: "Bridges");

            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Project_ProjectId",
                table: "Bridges");

            migrationBuilder.DropForeignKey(
                name: "FK_Bridges_Tags_TagId",
                table: "Bridges");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "Bridges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Bridges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Bridges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Bridges_Tags_TagId",
                table: "Bridges",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
