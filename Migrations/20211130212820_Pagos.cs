using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class Pagos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuscripcionUsuario_Pagos_PagoId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropIndex(
                name: "IX_SuscripcionUsuario_PagoId",
                table: "SuscripcionUsuario");

            migrationBuilder.RenameColumn(
                name: "PagoId",
                table: "SuscripcionUsuario",
                newName: "MedioDePagoId");

            migrationBuilder.AddColumn<int>(
                name: "MedioDePagoUsuarioId",
                table: "SuscripcionUsuario",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "Pagos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<bool>(
                name: "EsPayPal",
                table: "Pagos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdPagoDevolucion",
                table: "Pagos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MedioId",
                table: "Pagos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionDevolucion",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoSuscripcionId",
                table: "Pagos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Borrado",
                table: "MediosDePagos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Borrado",
                table: "EntidadesFinancieras",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DevolucionesPayPal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagoId = table.Column<int>(type: "int", nullable: false),
                    DevolucionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoDevolucion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaDevolucion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevolucionesPayPal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevolucionesPayPal_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "IdPago",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagosPayPal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagoId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCaptura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosPayPal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagosPayPal_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "IdPago",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "4a771350-86a3-4bd5-bc36-140d046cd767");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "68e14434-7834-4609-bcc1-67de022fc6b5", "AQAAAAEAACcQAAAAEOuELouHtq3fFxixiprxYbv9BwVOqdoCnFq0VDsFCDNCl9UxtgpMRPVo5O8t5wcRGg==", "9088fbda-d868-448c-9d8f-4569e1803f9a" });

            migrationBuilder.InsertData(
                table: "Parametros",
                columns: new[] { "Nombre", "Valor" },
                values: new object[] { "GananciaCreador", "0.9" });

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionUsuario_MedioDePagoUsuarioId",
                table: "SuscripcionUsuario",
                column: "MedioDePagoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_MedioId",
                table: "Pagos",
                column: "MedioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_TipoSuscripcionId",
                table: "Pagos",
                column: "TipoSuscripcionId");

            migrationBuilder.CreateIndex(
                name: "IX_DevolucionesPayPal_PagoId",
                table: "DevolucionesPayPal",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosPayPal_PagoId",
                table: "PagosPayPal",
                column: "PagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_MediosDePagos_MedioId",
                table: "Pagos",
                column: "MedioId",
                principalTable: "MediosDePagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_TipoSuscripcion_TipoSuscripcionId",
                table: "Pagos",
                column: "TipoSuscripcionId",
                principalTable: "TipoSuscripcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuscripcionUsuario_MediosDePagos_MedioDePagoUsuarioId",
                table: "SuscripcionUsuario",
                column: "MedioDePagoUsuarioId",
                principalTable: "MediosDePagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_MediosDePagos_MedioId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_TipoSuscripcion_TipoSuscripcionId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_SuscripcionUsuario_MediosDePagos_MedioDePagoUsuarioId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropTable(
                name: "DevolucionesPayPal");

            migrationBuilder.DropTable(
                name: "PagosPayPal");

            migrationBuilder.DropIndex(
                name: "IX_SuscripcionUsuario_MedioDePagoUsuarioId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_MedioId",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_TipoSuscripcionId",
                table: "Pagos");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Nombre",
                keyValue: "GananciaCreador");

            migrationBuilder.DropColumn(
                name: "MedioDePagoUsuarioId",
                table: "SuscripcionUsuario");

            migrationBuilder.DropColumn(
                name: "EsPayPal",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "IdPagoDevolucion",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "MedioId",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "ObservacionDevolucion",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "TipoSuscripcionId",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "Borrado",
                table: "MediosDePagos");

            migrationBuilder.DropColumn(
                name: "Borrado",
                table: "EntidadesFinancieras");

            migrationBuilder.RenameColumn(
                name: "MedioDePagoId",
                table: "SuscripcionUsuario",
                newName: "PagoId");

            migrationBuilder.AlterColumn<float>(
                name: "Monto",
                table: "Pagos",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionUsuario_PagoId",
                table: "SuscripcionUsuario",
                column: "PagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuscripcionUsuario_Pagos_PagoId",
                table: "SuscripcionUsuario",
                column: "PagoId",
                principalTable: "Pagos",
                principalColumn: "IdPago",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
