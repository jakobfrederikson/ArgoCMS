using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgoCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamAndProjectToJobModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeamID",
                table: "Jobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProjectID",
                table: "Jobs",
                column: "ProjectID");

            // Already exists
            //migrationBuilder.CreateIndex(
            //    name: "IX_Jobs_TeamID",
            //    table: "Jobs",
            //    column: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Projects_ProjectID",
                table: "Jobs",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectId");

            // Already exists
            //migrationBuilder.AddForeignKey(
            //    name: "FK_Jobs_Teams_TeamID",
            //    table: "Jobs",
            //    column: "TeamID",
            //    principalTable: "Teams",
            //    principalColumn: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Projects_ProjectID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Teams_TeamID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ProjectID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TeamID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "TeamID",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
