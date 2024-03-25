using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Seguridad.Migrations
{
    public partial class DtosInicialesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "Apellido", "Email", "Nombre", "Password", "Rol", "Status" },
                values: new object[] { 1, "admin", "root@gmail.com", "root", "0192023a7bbd73250516f069df18b500", "Administrador", (byte)1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1);
        }
    }
}
