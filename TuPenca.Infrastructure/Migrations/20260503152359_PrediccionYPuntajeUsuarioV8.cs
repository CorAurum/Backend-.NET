using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPenca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PrediccionYPuntajeUsuarioV8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioGanadorId",
                table: "Premios",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MontoEntrada",
                table: "PlantillasPenca",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PorcentajeComision",
                table: "PlantillasPenca",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PorcentajePremio1",
                table: "Pencas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PorcentajePremio2",
                table: "Pencas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PorcentajePremio3",
                table: "Pencas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Premios_UsuarioGanadorId",
                table: "Premios",
                column: "UsuarioGanadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Premios_Usuarios_UsuarioGanadorId",
                table: "Premios",
                column: "UsuarioGanadorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Premios_Usuarios_UsuarioGanadorId",
                table: "Premios");

            migrationBuilder.DropIndex(
                name: "IX_Premios_UsuarioGanadorId",
                table: "Premios");

            migrationBuilder.DropColumn(
                name: "UsuarioGanadorId",
                table: "Premios");

            migrationBuilder.DropColumn(
                name: "MontoEntrada",
                table: "PlantillasPenca");

            migrationBuilder.DropColumn(
                name: "PorcentajeComision",
                table: "PlantillasPenca");

            migrationBuilder.DropColumn(
                name: "PorcentajePremio1",
                table: "Pencas");

            migrationBuilder.DropColumn(
                name: "PorcentajePremio2",
                table: "Pencas");

            migrationBuilder.DropColumn(
                name: "PorcentajePremio3",
                table: "Pencas");
        }
    }
}
