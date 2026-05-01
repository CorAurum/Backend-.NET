using TuPenca.Application.DTOs.Usuario;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> ObtenerUsuariosAsync();
        Task<UsuarioResponseDto> ActualizarEstadoAsync(UsuarioActualizarEstadoRequestDto usuarioDto);
    }
}
