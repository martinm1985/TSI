using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class Creadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntidadFinanciera",
                table: "Creadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroDeCuenta",
                table: "Creadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "cbde229a-f053-4807-9fca-5d8c720c64c2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e23dd49d-3905-4b2e-9ce6-5f5f098d654e", "AQAAAAEAACcQAAAAEPuQv3c3aPudSbKH1scMS+gnVGuD7HlcUYcdfVKFOqUdHZ5r2yE6DQxsA5gZOHhsrg==", "81c04d0b-361a-4622-9f9d-4eb0095cdf2e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntidadFinanciera",
                table: "Creadores");

            migrationBuilder.DropColumn(
                name: "NumeroDeCuenta",
                table: "Creadores");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "d0e39971-ed4a-40b8-bf05-7c565d9662eb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2841487-d855-40cb-b214-d6fb69f93e78", "AQAAAAEAACcQAAAAEIGrs/J1GtUfEP+oAZVSXjbNosGynyGYx5GnIuqXDF9AlkEW+oJvMcwPDi3cZ/QSRg==", "d18d7a8a-e30a-41b4-90b7-b584e771690a" });
        }
    }
}
