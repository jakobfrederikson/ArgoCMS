using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgoCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoticeNotificationUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationGroupId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeId",
                table: "Notifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationGroupId",
                table: "Notifications",
                column: "NotificationGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_EmployeeId",
                table: "Notifications",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationGroups_NotificationGroupId",
                table: "Notifications",
                column: "NotificationGroupId",
                principalTable: "NotificationGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_EmployeeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationGroups_NotificationGroupId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_EmployeeId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationGroupId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationGroupId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
