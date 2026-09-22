using System;
using System.Collections.Generic;

namespace SMTQC.API.Models;

public partial class Productore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string? Comunidad { get; set; }

    public string? Telefono { get; set; }

    public int? UsuarioId { get; set; }

    public virtual Usuario? Usuario { get; set; }

    public virtual ICollection<Autorizacione> Autorizaciones { get; set; } = new List<Autorizacione>();
}
