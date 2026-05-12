using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPenca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMPPreferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MercadoPagoPreferenceId",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MercadoPagoPreferenceId",
                table: "Pagos");
        }
    }
}
