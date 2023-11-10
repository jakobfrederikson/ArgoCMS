using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgoCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoticeCommentNotificationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NoticeCommentNotification_CompanyWide",
                table: "Notifications",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoticeCommentNotification_NotificationGroupId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoticeCommentNotification_ProjectId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoticeCommentNotification_TeamId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NoticeCommentNotification_NotificationGroupId",
                table: "Notifications",
                column: "NoticeCommentNotification_NotificationGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationGroups_NoticeCommentNotification_NotificationGroupId",
                table: "Notifications",
                column: "NoticeCommentNotification_NotificationGroupId",
                principalTable: "NotificationGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationGroups_NoticeCommentNotification_NotificationGroupId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NoticeCommentNotification_NotificationGroupId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NoticeCommentNotification_CompanyWide",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NoticeCommentNotification_NotificationGroupId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NoticeCommentNotification_ProjectId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NoticeCommentNotification_TeamId",
                table: "Notifications");
        }
    }
}
