using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPenca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSitio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EsquemaColores",
                table: "Sitios",
                newName: "ColorSecundario");

            migrationBuilder.AddColumn<string>(
                name: "ColorPrimario",
                table: "Sitios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorPrimario",
                table: "Sitios");

            migrationBuilder.RenameColumn(
                name: "ColorSecundario",
                table: "Sitios",
                newName: "EsquemaColores");
        }
    }
}
