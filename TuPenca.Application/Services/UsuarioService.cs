using TuPenca.Application.DTOs.Sitio;
using TuPenca.Application.DTOs.Usuario;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerUsuariosAsync()
        {
            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            var result = new List<UsuarioDto>();
            foreach (var usuario in usuarios)
            {
                result.Add(new UsuarioDto()
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    FechaRegistro = usuario.FechaRegistro,
                    Estado = usuario.Estado,
                    SitioId = usuario.SitioId
                });
            }
            return result;
        }

        public async Task<UsuarioResponseDto> ActualizarEstadoAsync(UsuarioActualizarEstadoRequestDto usuarioDto)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioDto.Id);
            if (usuario == null)
                return new UsuarioResponseDto { Mensaje = "Usuario no encontrado" };

            if (usuarioDto.Estado == EstadoUsuario.Aprobado)
                usuario.Estado = EstadoUsuario.Aprobado;
            else
                usuario.Estado = EstadoUsuario.Rechazado;

            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Mensaje = "Usuario actualizado exitosamente"
            };
        }

    }
}
