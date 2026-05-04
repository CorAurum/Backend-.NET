using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TuPenca.Application.Tests
{
    public class FlujoPencaTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly TestHttpClient _client;

        public FlujoPencaTests(WebApplicationFactory<Program> factory)
        {
            _client = new TestHttpClient(factory.CreateClient());
        }

        [Fact]
        public async Task Flujo_Completo_Deberia_Funcionar()
        {
            // =========================
            // LOGIN ADMIN PLATAFORMA
            // =========================
            var loginAdmin = await _client.Post("/api/auth/login", new
            {
                email = "admin@tupenca.uy",
                password = "1234"
            });

            var tokenAdmin = JsonHelpers.GetString(loginAdmin, "token");
            _client.SetBearer(tokenAdmin);

            // =========================
            // CREAR DEPORTE
            // =========================
            var deporteJson = await _client.Post("/api/deporte/crear", new
            {
                nombres = new[] { "Futbol" }
            });

            var deporteId = JsonHelpers.ExtractFirstId(deporteJson);

            // =========================
            // CREAR TIPO COMPETENCIA
            // =========================
            var tipoJson = await _client.Post("/api/tipocompetencia/crear", new
            {
                nombres = new[] { "Liga" }
            });

            var tipoCompetenciaId = JsonHelpers.ExtractFirstId(tipoJson);

            // =========================
            // CREAR EQUIPOS
            // =========================
            var equiposJson = await _client.Post("/api/equipo/crear", new
            {
                nombres = new[] { "Uruguay", "Argentina" }
            });

            var uruguayId = JsonHelpers.ExtractFirstId(equiposJson);
            var argentinaId = JsonHelpers.ExtractSecondId(equiposJson);

            // =========================
            // CREAR EVENTO
            // =========================
            var eventoJson = await _client.Post("/api/eventodeportivo/crear", new
            {
                nombre = "Copa Test",
                fechaInicio = DateTime.UtcNow,
                fechaFin = DateTime.UtcNow.AddDays(10),
                deporteId = deporteId,
                tipoCompetenciaId = tipoCompetenciaId
            });

            var eventoId = JsonHelpers.ExtractFirstId(eventoJson);

            // =========================
            // CREAR PARTIDO
            // =========================
            var partidoJson = await _client.Post("/api/eventodeportivo/partido/agregar", new
            {
                fecha = DateTime.UtcNow.AddDays(1),
                fase = "Grupos",
                equipoLocalId = uruguayId,
                equipoVisitanteId = argentinaId,
                eventoDeportivoId = eventoId
            });

            var partidoId = JsonHelpers.ExtractFirstId(partidoJson);

            // =========================
            // CREAR PLANTILLA
            // =========================
            var plantillaJson = await _client.Post("/api/plantillapenca/crear", new
            {
                nombre = "Plantilla Test",
                descripcion = "Test",
                tiempoLimitePrevioMinutos = 60,
                montoEntrada = 100,
                porcentajeComision = 10,
                eventoDeportivoId = eventoId,
                reglas = new[]
                {
                new { desviacion = 0, puntaje = 10 },
                new { desviacion = 1, puntaje = 5 }
            }
            });

            var plantillaId = JsonHelpers.ExtractFirstId(plantillaJson);

            // =========================
            // LOGIN ADMIN SITIO
            // =========================
            _client.ClearHeaders();
            _client.SetHeader("X-Sitio", "prueba.tupenca.uy");

            var loginSitio = await _client.Post("/api/auth/login", new
            {
                email = "adminsitio@prueba.tupenca.uy",
                password = "1234"
            });

            var tokenAdminSitio = JsonHelpers.GetString(loginSitio, "token");
            _client.SetBearer(tokenAdminSitio);

            // =========================
            // CREAR PENCA
            // =========================
            var pencaJson = await _client.Post("/api/penca/crear", new
            {
                nombre = "Penca Test",
                plantillaPencaId = plantillaId,
                porcentajePremio1 = 60,
                porcentajePremio2 = 20,
                porcentajePremio3 = 10
            });

            var pencaId = JsonHelpers.ExtractFirstId(pencaJson);

            // =========================
            // LOGIN USUARIO
            // =========================
            _client.ClearHeaders();
            _client.SetHeader("X-Sitio", "prueba.tupenca.uy");

            var loginUser = await _client.Post("/api/auth/login", new
            {
                email = "usuario@prueba.tupenca.uy",
                password = "1234"
            });

            var tokenUsuario = JsonHelpers.GetString(loginUser, "token");
            _client.SetBearer(tokenUsuario);

            // =========================
            // PAGO
            // =========================
            await _client.Post("/api/pago/realizar", new
            {
                pencaId = pencaId,
                monto = 100
            });

            // =========================
            // PREDICCION
            // =========================
            await _client.Post("/api/prediccion", new
            {
                pencaId = pencaId,
                partidoId = partidoId,
                golesLocal = 2,
                golesVisitante = 1
            });

            // =========================
            // RESULTADO
            // =========================
            _client.ClearHeaders();
            _client.SetBearer(tokenAdmin);

            await _client.Post("/api/eventodeportivo/resultado/cargar", new
            {
                partidoId = partidoId,
                golesLocal = 2,
                golesVisitante = 1
            });

            // =========================
            // TABLA
            // =========================
            _client.ClearHeaders();
            _client.SetBearer(tokenUsuario);
            _client.SetHeader("X-Sitio", "prueba.tupenca.uy");

            var tabla = await _client.Get($"/api/penca/{pencaId}/tabla-posiciones");
            tabla.Should().NotBeNull();

            // =========================
            // CERRAR PENCA
            // =========================
            _client.ClearHeaders();
            _client.SetBearer(tokenAdminSitio);
            _client.SetHeader("X-Sitio", "prueba.tupenca.uy");

            await _client.Put($"/api/penca/{pencaId}/estado", 1);

            // =========================
            // GANADORES
            // =========================
            _client.ClearHeaders();
            _client.SetBearer(tokenUsuario);
            _client.SetHeader("X-Sitio", "prueba.tupenca.uy");

            var ganadores = await _client.Get($"/api/penca/{pencaId}/ganadores");
            ganadores.Should().NotBeNull();
        }
    }
}
