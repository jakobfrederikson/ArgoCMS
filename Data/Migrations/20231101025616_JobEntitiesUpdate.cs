using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgoCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class JobEntitiesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_AspNetUsers_EmployeesId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_Projects_ProjectsProjectId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_EmployeeID",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "ProjectTeam");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Jobs",
                newName: "AssignedEmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_EmployeeID",
                table: "Jobs",
                newName: "IX_Jobs_AssignedEmployeeID");

            migrationBuilder.RenameColumn(
                name: "ProjectsProjectId",
                table: "EmployeeProject",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "EmployeesId",
                table: "EmployeeProject",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeProject_ProjectsProjectId",
                table: "EmployeeProject",
                newName: "IX_EmployeeProject_ProjectId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Teams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamLeaderId",
                table: "Teams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProjectStatus",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Notices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Notices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "Notices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "EmployeeProject",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TeamProject",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamProject", x => new { x.TeamId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_TeamProject_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamProject_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CreatedById",
                table: "Teams",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamLeaderId",
                table: "Teams",
                column: "TeamLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerID",
                table: "Projects",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_OwnerID",
                table: "Notices",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_ProjectId",
                table: "Notices",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_TeamId",
                table: "Notices",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_OwnerID",
                table: "Jobs",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamProject_ProjectId",
                table: "TeamProject",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_AspNetUsers_EmployeeId",
                table: "EmployeeProject",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_Projects_ProjectId",
                table: "EmployeeProject",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_AssignedEmployeeID",
                table: "Jobs",
                column: "AssignedEmployeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_OwnerID",
                table: "Jobs",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notices_AspNetUsers_OwnerID",
                table: "Notices",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notices_Projects_ProjectId",
                table: "Notices",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notices_Teams_TeamId",
                table: "Notices",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_OwnerID",
                table: "Projects",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_CreatedById",
                table: "Teams",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_TeamLeaderId",
                table: "Teams",
                column: "TeamLeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_AspNetUsers_EmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_Projects_ProjectId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_AssignedEmployeeID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_OwnerID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Notices_AspNetUsers_OwnerID",
                table: "Notices");

            migrationBuilder.DropForeignKey(
                name: "FK_Notices_Projects_ProjectId",
                table: "Notices");

            migrationBuilder.DropForeignKey(
                name: "FK_Notices_Teams_TeamId",
                table: "Notices");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_OwnerID",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_CreatedById",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_TeamLeaderId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "TeamProject");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CreatedById",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TeamLeaderId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OwnerID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Notices_OwnerID",
                table: "Notices");

            migrationBuilder.DropIndex(
                name: "IX_Notices_ProjectId",
                table: "Notices");

            migrationBuilder.DropIndex(
                name: "IX_Notices_TeamId",
                table: "Notices");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_OwnerID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamLeaderId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectStatus",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "EmployeeProject");

            migrationBuilder.RenameColumn(
                name: "AssignedEmployeeID",
                table: "Jobs",
                newName: "EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_AssignedEmployeeID",
                table: "Jobs",
                newName: "IX_Jobs_EmployeeID");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "EmployeeProject",
                newName: "ProjectsProjectId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeProject",
                newName: "EmployeesId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeProject_ProjectId",
                table: "EmployeeProject",
                newName: "IX_EmployeeProject_ProjectsProjectId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Notices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Notices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "Notices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ProjectTeam",
                columns: table => new
                {
                    ProjectsProjectId = table.Column<int>(type: "int", nullable: false),
                    TeamsTeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeam", x => new { x.ProjectsProjectId, x.TeamsTeamId });
                    table.ForeignKey(
                        name: "FK_ProjectTeam_Projects_ProjectsProjectId",
                        column: x => x.ProjectsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_Teams_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_TeamsTeamId",
                table: "ProjectTeam",
                column: "TeamsTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_AspNetUsers_EmployeesId",
                table: "EmployeeProject",
                column: "EmployeesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_Projects_ProjectsProjectId",
                table: "EmployeeProject",
                column: "ProjectsProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_EmployeeID",
                table: "Jobs",
                column: "EmployeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
