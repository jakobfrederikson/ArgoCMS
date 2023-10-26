using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgoCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoticesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Notices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PublicityStatus",
                table: "Notices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Notices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TeamID",
                table: "Jobs",
                column: "TeamID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Teams_TeamID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TeamID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "PublicityStatus",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Notices");
        }
    }
}
