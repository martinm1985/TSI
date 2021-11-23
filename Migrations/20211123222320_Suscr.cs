using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class Suscr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Parametros",
                columns: new[] { "Nombre", "Valor" },
                values: new object[,]
                {
                    { "SUSCDEFECTO1", "1" },
                    { "SUSCDEFECTO2", "2" },
                    { "SUSCDEFECTO3", "3" }
                });

            migrationBuilder.InsertData(
                table: "TipoSuscripcion",
                columns: new[] { "Id", "Activo", "Beneficios", "CreadorId", "Imagen", "IncluyeTipoSuscrId", "MensajeBienvenida", "MensajeriaActiva", "Nombre", "Precio", "VideoBienvenida" },
                values: new object[] { 1, true, "Acceso a un nuevo contenido por semana;;", null, "TODO", null, "", false, "Basico", 1f, false });

            migrationBuilder.InsertData(
                table: "TipoSuscripcion",
                columns: new[] { "Id", "Activo", "Beneficios", "CreadorId", "Imagen", "IncluyeTipoSuscrId", "MensajeBienvenida", "MensajeriaActiva", "Nombre", "Precio", "VideoBienvenida" },
                values: new object[] { 2, true, "Acceso a todo el contenido subido;;", null, "TODO", 1, "", false, "Estandar", 5f, false });

            migrationBuilder.InsertData(
                table: "TipoSuscripcion",
                columns: new[] { "Id", "Activo", "Beneficios", "CreadorId", "Imagen", "IncluyeTipoSuscrId", "MensajeBienvenida", "MensajeriaActiva", "Nombre", "Precio", "VideoBienvenida" },
                values: new object[] { 3, true, "Todo lo que incluye el estandar;;CHATEA CONMIGO;;", null, "TODO", 2, "", true, "Estandar", 10f, false });

            migrationBuilder.CreateIndex(
                name: "IX_MediosDePagos_IdEntidadFinanciera",
                table: "MediosDePagos",
                column: "IdEntidadFinanciera");

            migrationBuilder.AddForeignKey(
                name: "FK_MediosDePagos_EntidadesFinancieras_IdEntidadFinanciera",
                table: "MediosDePagos",
                column: "IdEntidadFinanciera",
                principalTable: "EntidadesFinancieras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediosDePagos_EntidadesFinancieras_IdEntidadFinanciera",
                table: "MediosDePagos");

            migrationBuilder.DropIndex(
                name: "IX_MediosDePagos_IdEntidadFinanciera",
                table: "MediosDePagos");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Nombre",
                keyValue: "SUSCDEFECTO1");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Nombre",
                keyValue: "SUSCDEFECTO2");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Nombre",
                keyValue: "SUSCDEFECTO3");

            migrationBuilder.DeleteData(
                table: "TipoSuscripcion",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoSuscripcion",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoSuscripcion",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "3732a745-26f4-4926-8e60-3f6cd0dd60a9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fb937ece-7151-4f36-89ad-e8f5ace7d9cf", "AQAAAAEAACcQAAAAEI4q+Lmcyqm5/9UlRJgB7Szr24IFGe7gkx6l77XKsxGoGnejKPjebMI2ZdLYrGzpdA==", "5703e212-e339-4bf3-9781-935fdfa83758" });
        }
    }
}
