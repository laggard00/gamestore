using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class anotha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GamePlatforms_PlatformId_GameId",
                table: "GamePlatforms");

            migrationBuilder.DropIndex(
                name: "IX_GameGenre_GenreId_GameId",
                table: "GameGenre");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlatforms",
                table: "GamePlatforms",
                columns: new[] { "PlatformId", "GameId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre",
                columns: new[] { "GenreId", "GameId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlatforms",
                table: "GamePlatforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatforms_PlatformId_GameId",
                table: "GamePlatforms",
                columns: new[] { "PlatformId", "GameId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GenreId_GameId",
                table: "GameGenre",
                columns: new[] { "GenreId", "GameId" },
                unique: true);
        }
    }
}
