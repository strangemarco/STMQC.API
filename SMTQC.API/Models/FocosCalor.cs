using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace SMTQC.API.Models;

public partial class FocosCalor
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public Geometry? Ubicacion { get; set; }

    public double Latitud { get; set; }

    public double Longitud { get; set; }

    public string? Sensor { get; set; }

    public double Confianza { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Alerta> Alerta { get; set; } = new List<Alerta>();
}
