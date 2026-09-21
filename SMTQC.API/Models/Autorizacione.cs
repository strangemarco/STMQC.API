using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace SMTQC.API.Models;

public partial class Autorizacione
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public double Superficie { get; set; }

    public Geometry? Geometria { get; set; }

    public string Estado { get; set; } = null!;

    public int ProductorId { get; set; }

    public virtual Productore Productor { get; set; } = null!;
}
