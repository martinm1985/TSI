using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class Devoluciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Devuelto",
                table: "Pagos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "bba3b178-cdb5-45e3-a44a-8b7139cd87bc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "477096ec-6217-43a3-a15d-fb24a73f936f", "AQAAAAEAACcQAAAAECy45K6wsHZ67R/N/EMkP+Dyn12/pxB9ZI2pw6oOhknQUme+0wdmp9sKzxm1LA/ewA==", "0d14d525-bceb-42b8-b281-73a5a63a5bf9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Devuelto",
                table: "Pagos");

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
    }
}
