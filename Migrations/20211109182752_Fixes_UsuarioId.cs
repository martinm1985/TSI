using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class Fixes_UsuarioId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversacion_AspNetUsers_UserId",
                table: "Conversacion");

            migrationBuilder.DropForeignKey(
                name: "FK_SuscripcionUsuario_AspNetUsers_UserId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropIndex(
                name: "IX_SuscripcionUsuario_UserId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropIndex(
                name: "IX_Conversacion_UserId",
                table: "Conversacion");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Conversacion");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "SuscripcionUsuario",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Conversacion",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionUsuario_UsuarioId",
                table: "SuscripcionUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversacion_UsuarioId",
                table: "Conversacion",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversacion_AspNetUsers_UsuarioId",
                table: "Conversacion",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuscripcionUsuario_AspNetUsers_UsuarioId",
                table: "SuscripcionUsuario",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversacion_AspNetUsers_UsuarioId",
                table: "Conversacion");

            migrationBuilder.DropForeignKey(
                name: "FK_SuscripcionUsuario_AspNetUsers_UsuarioId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropIndex(
                name: "IX_SuscripcionUsuario_UsuarioId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropIndex(
                name: "IX_Conversacion_UsuarioId",
                table: "Conversacion");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "SuscripcionUsuario",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SuscripcionUsuario",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Conversacion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Conversacion",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionUsuario_UserId",
                table: "SuscripcionUsuario",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversacion_UserId",
                table: "Conversacion",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversacion_AspNetUsers_UserId",
                table: "Conversacion",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuscripcionUsuario_AspNetUsers_UserId",
                table: "SuscripcionUsuario",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
