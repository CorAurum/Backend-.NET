using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;
using TuPenca.Application.DTOs.Pago;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TuPenca.Application.Services
{
    public class PagoService : IPagoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PagoService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration; // ← nuevo
        }

        // METODO DE PAGO DE PRUEBA LOCAL - REMOVER CUANDO CORRESPONDA
        public async Task<PagoResponseDto> RealizarPagoAsync(PagoRequestDto dto, Guid usuarioId)
        {
            var penca = await _unitOfWork.Pencas.GetByIdAsync(dto.PencaId);
            if (penca == null)
                throw new Exception("Penca no encontrada");

            if (penca.Estado != EstadoPenca.Abierta)
                throw new Exception("La penca no está abierta para nuevos participantes");

            // Verificar que no haya pagado antes
            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            var pagoExistente = pagos.FirstOrDefault(p =>
                p.UsuarioId == usuarioId &&
                p.PencaId == dto.PencaId &&
                p.Estado == EstadoPago.Aprobado);

            if (pagoExistente != null)
                throw new Exception("Ya estás inscripto en esta penca");

            // Por ahora simulamos pago exitoso
            var pago = new Pago
            {
                Id = Guid.NewGuid(),
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow,
                Estado = EstadoPago.Aprobado,
                UsuarioId = usuarioId,
                PencaId = dto.PencaId
            };

            await _unitOfWork.Pagos.AddAsync(pago);
            await _unitOfWork.SaveChangesAsync();

            return new PagoResponseDto
            {
                Id = pago.Id,
                PencaId = pago.PencaId,
                UsuarioId = pago.UsuarioId,
                Monto = pago.Monto,
                Estado = pago.Estado,
                Fecha = pago.Fecha
            };
        }

        // METODOS MERCADO PAGO PREFERENCE Y WEBHOOK ---------------------------------------------------------------------------------------------
        public async Task<string> IniciarPagoAsync(Guid pencaId, Guid usuarioId)
        {
            // 1. Verificar que la penca existe
            var penca = await _unitOfWork.Pencas.GetByIdConDetalleAsync(pencaId);
            if (penca == null)
                throw new Exception("Penca no encontrada");

            // 2. Verificar que el usuario no pagó ya
            var yaIscrip = await UsuarioPagoEnPencaAsync(usuarioId, pencaId);
            if (yaIscrip)
                throw new Exception("Ya estás inscripto en esta penca");

            // 3. Crear preferencia en MercadoPago
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
        {
            new PreferenceItemRequest
            {
                Title = $"Inscripción penca: {penca.Nombre}",
                Quantity = 1,
                CurrencyId = "UYU",
                UnitPrice = penca.Plantilla.MontoEntrada
            }
        },
                Metadata = new Dictionary<string, object>
        {
            { "penca_id", pencaId.ToString() },
            { "usuario_id", usuarioId.ToString() }
        },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://tuapp.com/pago/exitoso",
                    Failure = "https://tuapp.com/pago/fallido",
                    Pending = "https://tuapp.com/pago/pendiente"
                },
                NotificationUrl = $"{_configuration["App:BaseUrl"]}/api/pago/webhook"
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            // 4. Guardar el pago como Pendiente en tu tabla
            var pago = new Pago
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                PencaId = pencaId,
                Monto = penca.Plantilla.MontoEntrada,
                Estado = EstadoPago.Pendiente,
                MercadoPagoPreferenceId = preference.Id // ← necesitás agregar este campo a la entidad Pago
            };

            await _unitOfWork.Pagos.AddAsync(pago);
            await _unitOfWork.SaveChangesAsync();

            return preference.InitPoint; // ← URL a la que redirigís al usuario
        }



        public async Task ProcesarWebhookAsync(long mercadoPagoPaymentId)
        {
            // 1. Consultar el pago a MercadoPago para verificarlo
            var client = new PaymentClient();
            var payment = await client.GetAsync(mercadoPagoPaymentId);

            if (payment == null)
                throw new Exception("Pago no encontrado en MercadoPago");

            // 2. Extraer pencaId y usuarioId del metadata
            var pencaId = Guid.Parse(payment.Metadata["penca_id"].ToString()!);
            var usuarioId = Guid.Parse(payment.Metadata["usuario_id"].ToString()!);

            // 3. Buscar el pago pendiente en tu tabla
            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            var pago = pagos.FirstOrDefault(p =>
                p.PencaId == pencaId &&
                p.UsuarioId == usuarioId &&
                p.Estado == EstadoPago.Pendiente);

            if (pago == null) return;

            // 4. Actualizar estado según respuesta de MP
            pago.Estado = payment.Status switch
            {
                "approved" => EstadoPago.Aprobado,
                "rejected" => EstadoPago.Rechazado,
                _ => EstadoPago.Pendiente
            };

            await _unitOfWork.Pagos.UpdateAsync(pago);
            await _unitOfWork.SaveChangesAsync();
        }

        // ---------------------------------------------------------------------------------------------

        public async Task<bool> UsuarioPagoEnPencaAsync(Guid usuarioId, Guid pencaId)
        {
            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            return pagos.Any(p =>
                p.UsuarioId == usuarioId &&
                p.PencaId == pencaId &&
                p.Estado == EstadoPago.Aprobado);
        }
    }
}