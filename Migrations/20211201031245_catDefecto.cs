using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class catDefecto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "3870dbc9-ed1b-4271-800e-3a9e45fa4605");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7321faff-dbc7-43a7-8ebc-194c7c9f92bf", "AQAAAAEAACcQAAAAEMTkZf8FYc0fad10dZlVCk2gVtc5VxdcgT0M2H7Nn1tspZC4fJiNyxlZdEQBRSCnbQ==", "5cc8d9de-f739-48cb-96b1-08d4b20c833e" });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Fotografia" },
                    { 2, "Videos" }
                });

            migrationBuilder.UpdateData(
                table: "TipoSuscripcion",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "Premium");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.UpdateData(
                table: "TipoSuscripcion",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "Estandar");
        }
    }
}
