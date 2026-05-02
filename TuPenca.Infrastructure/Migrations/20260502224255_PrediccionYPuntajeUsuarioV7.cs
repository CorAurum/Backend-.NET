using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPenca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PrediccionYPuntajeUsuarioV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PencaId",
                table: "Pagos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_PencaId",
                table: "Pagos",
                column: "PencaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Pencas_PencaId",
                table: "Pagos",
                column: "PencaId",
                principalTable: "Pencas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Pencas_PencaId",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_PencaId",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "PencaId",
                table: "Pagos");
        }
    }
}
