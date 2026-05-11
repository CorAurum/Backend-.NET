using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPenca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test50 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EquipoGanadorPredichoId",
                table: "Predicciones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PuntajeGanador",
                table: "PlantillasPenca",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipoGanadorId",
                table: "Partidos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predicciones_EquipoGanadorPredichoId",
                table: "Predicciones",
                column: "EquipoGanadorPredichoId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EquipoGanadorId",
                table: "Partidos",
                column: "EquipoGanadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Equipos_EquipoGanadorId",
                table: "Partidos",
                column: "EquipoGanadorId",
                principalTable: "Equipos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Predicciones_Equipos_EquipoGanadorPredichoId",
                table: "Predicciones",
                column: "EquipoGanadorPredichoId",
                principalTable: "Equipos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Equipos_EquipoGanadorId",
                table: "Partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Predicciones_Equipos_EquipoGanadorPredichoId",
                table: "Predicciones");

            migrationBuilder.DropIndex(
                name: "IX_Predicciones_EquipoGanadorPredichoId",
                table: "Predicciones");

            migrationBuilder.DropIndex(
                name: "IX_Partidos_EquipoGanadorId",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "EquipoGanadorPredichoId",
                table: "Predicciones");

            migrationBuilder.DropColumn(
                name: "PuntajeGanador",
                table: "PlantillasPenca");

            migrationBuilder.DropColumn(
                name: "EquipoGanadorId",
                table: "Partidos");
        }
    }
}
