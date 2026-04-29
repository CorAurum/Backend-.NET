using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPenca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntidadesPencaV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoAcierto",
                table: "ReglasPuntuacion");

            migrationBuilder.AddColumn<int>(
                name: "Desviacion",
                table: "ReglasPuntuacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TiempoLimitePrevioMinutos",
                table: "PlantillasPenca",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Pencas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desviacion",
                table: "ReglasPuntuacion");

            migrationBuilder.DropColumn(
                name: "TiempoLimitePrevioMinutos",
                table: "PlantillasPenca");

            migrationBuilder.AddColumn<string>(
                name: "TipoAcierto",
                table: "ReglasPuntuacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Pencas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
