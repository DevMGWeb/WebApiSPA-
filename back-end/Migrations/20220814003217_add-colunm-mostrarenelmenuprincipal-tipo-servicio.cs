using Microsoft.EntityFrameworkCore.Migrations;

namespace back_end.Migrations
{
    public partial class addcolunmmostrarenelmenuprincipaltiposervicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MostrarEnElMenuPrincipal",
                table: "TipoServicios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MostrarEnElMenuPrincipal",
                table: "TipoServicios");
        }
    }
}
