using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class please : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId1",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_GenreId1",
                table: "Games",
                column: "GenreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Genres_GenreId1",
                table: "Games",
                column: "GenreId1",
                principalTable: "Genres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Genres_GenreId1",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GenreId1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GenreId1",
                table: "Games");
        }
    }
}
