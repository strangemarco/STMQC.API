using System;
using System.Collections.Generic;

namespace SMTQC.API.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
