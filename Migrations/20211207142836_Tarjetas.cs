using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class Tarjetas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "MediosDePagos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "5472c578-ccb6-4858-aaf3-52c0e7a9b664");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a032e02-3370-4c82-8bcf-e9c375841775", "AQAAAAEAACcQAAAAEDGqaQxQNDd2dVl/uS+UtDURUpcVNVNwcDYmVABe40uhG8KOEavTi+v9OBlTf8ifSA==", "cbd6291d-ec45-4a97-8639-0b62b72c26b7" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "MediosDePagos");

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
        }
    }
}
