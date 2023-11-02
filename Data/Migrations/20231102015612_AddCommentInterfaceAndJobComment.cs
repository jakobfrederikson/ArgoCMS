using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgoCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentInterfaceAndJobComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_AspNetUsers_EmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_Projects_ProjectId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamProject_Projects_ProjectId",
                table: "TeamProject");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamProject_Teams_TeamId",
                table: "TeamProject");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamProject",
                table: "TeamProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject");

            migrationBuilder.RenameTable(
                name: "TeamProject",
                newName: "TeamProjects");

            migrationBuilder.RenameTable(
                name: "EmployeeProject",
                newName: "EmployeesProjects");

            migrationBuilder.RenameIndex(
                name: "IX_TeamProject_ProjectId",
                table: "TeamProjects",
                newName: "IX_TeamProjects_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeProject_ProjectId",
                table: "EmployeesProjects",
                newName: "IX_EmployeesProjects_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamProjects",
                table: "TeamProjects",
                columns: new[] { "TeamId", "ProjectId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeesProjects",
                table: "EmployeesProjects",
                columns: new[] { "EmployeeId", "ProjectId" });

            migrationBuilder.CreateTable(
                name: "JobComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_JobComments_Jobs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoticeComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticeComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_NoticeComments_Notices_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Notices",
                        principalColumn: "NoticeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobComments_ParentId",
                table: "JobComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NoticeComments_ParentId",
                table: "NoticeComments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesProjects_AspNetUsers_EmployeeId",
                table: "EmployeesProjects",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesProjects_Projects_ProjectId",
                table: "EmployeesProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamProjects_Projects_ProjectId",
                table: "TeamProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamProjects_Teams_TeamId",
                table: "TeamProjects",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesProjects_AspNetUsers_EmployeeId",
                table: "EmployeesProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesProjects_Projects_ProjectId",
                table: "EmployeesProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamProjects_Projects_ProjectId",
                table: "TeamProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamProjects_Teams_TeamId",
                table: "TeamProjects");

            migrationBuilder.DropTable(
                name: "JobComments");

            migrationBuilder.DropTable(
                name: "NoticeComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamProjects",
                table: "TeamProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeesProjects",
                table: "EmployeesProjects");

            migrationBuilder.RenameTable(
                name: "TeamProjects",
                newName: "TeamProject");

            migrationBuilder.RenameTable(
                name: "EmployeesProjects",
                newName: "EmployeeProject");

            migrationBuilder.RenameIndex(
                name: "IX_TeamProjects_ProjectId",
                table: "TeamProject",
                newName: "IX_TeamProject_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeesProjects_ProjectId",
                table: "EmployeeProject",
                newName: "IX_EmployeeProject_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamProject",
                table: "TeamProject",
                columns: new[] { "TeamId", "ProjectId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject",
                columns: new[] { "EmployeeId", "ProjectId" });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoticeId = table.Column<int>(type: "int", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Notices_NoticeId",
                        column: x => x.NoticeId,
                        principalTable: "Notices",
                        principalColumn: "NoticeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NoticeId",
                table: "Comments",
                column: "NoticeId");

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
                name: "FK_TeamProject_Projects_ProjectId",
                table: "TeamProject",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamProject_Teams_TeamId",
                table: "TeamProject",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
