using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgoCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "NoticeComments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "JobComments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_NoticeComments_OwnerID",
                table: "NoticeComments",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_JobComments_OwnerID",
                table: "JobComments",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobComments_AspNetUsers_OwnerID",
                table: "JobComments",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoticeComments_AspNetUsers_OwnerID",
                table: "NoticeComments",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobComments_AspNetUsers_OwnerID",
                table: "JobComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NoticeComments_AspNetUsers_OwnerID",
                table: "NoticeComments");

            migrationBuilder.DropIndex(
                name: "IX_NoticeComments_OwnerID",
                table: "NoticeComments");

            migrationBuilder.DropIndex(
                name: "IX_JobComments_OwnerID",
                table: "JobComments");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "NoticeComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "JobComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
