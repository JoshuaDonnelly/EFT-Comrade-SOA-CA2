using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA_2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueGameIdConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quests_GameId",
                table: "Quests");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_GameId",
                table: "Quests",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quests_GameId",
                table: "Quests");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_GameId",
                table: "Quests",
                column: "GameId",
                unique: true);
        }
    }
}
