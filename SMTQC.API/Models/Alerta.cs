using System;
using System.Collections.Generic;

namespace SMTQC.API.Models;

public partial class Alerta
{
    public int Id { get; set; }

    public string Tipo { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string Ubicacion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public int? FocoCalorId { get; set; }

    public virtual FocosCalor? FocoCalor { get; set; }
}
