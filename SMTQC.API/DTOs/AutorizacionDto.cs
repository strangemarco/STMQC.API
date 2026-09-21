using System;

namespace SMTQC.API.DTOs
{
    public class AutorizacionDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double Superficie { get; set; }
        public string Estado { get; set; } = null!;
        public int ProductorId { get; set; }
        public string? GeometriaWkt { get; set; } // Representación en texto de la geometría
    }

    public class AutorizacionCreateDto
    {
        public string Codigo { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double Superficie { get; set; }
        public int ProductorId { get; set; }
        public string? GeometriaWkt { get; set; } // WKT (Well-Known Text) ej: POLYGON((...))
    }
}
