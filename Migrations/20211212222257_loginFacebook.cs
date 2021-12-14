using Microsoft.EntityFrameworkCore.Migrations;

namespace Crud.Migrations
{
    public partial class loginFacebook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdFacebook",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdFacebook",
                table: "AspNetUsers");

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
    }
}
