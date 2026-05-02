namespace TuPenca.Application.DTOs.Prediccion
{
    public class PrediccionRequestDto
    {
        public Guid PencaId { get; set; }
        public Guid PartidoId { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
    }
}