using System;

namespace SMTQC.API.DTOs
{
    public class ProductorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public string? Comunidad { get; set; }
        public string? Telefono { get; set; }
    }
}
