using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class chatsender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserSender",
                table: "Mensaje",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f32cc887-f5af-4748-9f88-22f427c9a7b2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80973f87-5a63-47b4-b13e-fa3ac7288b7c", "AQAAAAEAACcQAAAAEGY4hsQMzxCEECYHructoUvm6m7syfdwa520BPI+sBoObMLYKrnBJWi0qZZGgNSfZg==", "d65e4ebe-6d7f-4d8e-b3aa-62d4d1b74544" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSender",
                table: "Mensaje");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e64e6abd-a209-4078-a19c-38989a52f7ed");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a5d5104a-5729-4252-af6c-c4f496a01984", "AQAAAAEAACcQAAAAENXiWf5/FvjGCDCMQAbtOIvwWx4kxJyQueKjhUIx9lq5QjJ4mEMa/bolJgXkzPmmqQ==", "bbeb372a-2d7b-4dd0-99d6-afa6c5f71913" });
        }
    }
}
