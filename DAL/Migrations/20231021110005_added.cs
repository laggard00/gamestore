using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreEntityId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_GenreEntityId",
                table: "Genres",
                column: "GenreEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Genres_GenreEntityId",
                table: "Genres",
                column: "GenreEntityId",
                principalTable: "Genres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Genres_GenreEntityId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_GenreEntityId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "GenreEntityId",
                table: "Genres");
        }
    }
}
