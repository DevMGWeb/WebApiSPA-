using Microsoft.EntityFrameworkCore.Migrations;

namespace back_end.Migrations
{
    public partial class addcolunmpostertiposervicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "TipoServicios",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poster",
                table: "TipoServicios");
        }
    }
}
