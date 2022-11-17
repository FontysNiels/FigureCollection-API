using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FigureCollection.Migrations
{
    public partial class FigureImage2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Figures_Figure",
                table: "Collections");

            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Users_User",
                table: "Collections");

            migrationBuilder.CreateTable(
                name: "FigureImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FigureId = table.Column<int>(type: "int", nullable: false),
                    imgData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FigureImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_FigureImages_Figures_FigureId",
                        column: x => x.FigureId,
                        principalTable: "Figures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FigureImages_FigureId",
                table: "FigureImages",
                column: "FigureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Figures_Figure",
                table: "Collections",
                column: "Figure",
                principalTable: "Figures",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Users_User",
                table: "Collections",
                column: "User",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Figures_Figure",
                table: "Collections");

            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Users_User",
                table: "Collections");

            migrationBuilder.DropTable(
                name: "FigureImages");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Figures_Figure",
                table: "Collections",
                column: "Figure",
                principalTable: "Figures",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Users_User",
                table: "Collections",
                column: "User",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
