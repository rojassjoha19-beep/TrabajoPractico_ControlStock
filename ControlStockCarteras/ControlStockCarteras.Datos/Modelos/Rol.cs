using System;
using System.Collections.Generic;
using System.Text;

// Rol.cs
using System.ComponentModel.DataAnnotations;

namespace ControlStockCarteras.Datos.Modelos
{
    public class Rol
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; } = "";

        public List<UsuarioRol> UsuariosRoles { get; set; } = new();
        public List<RolAccion> RolesAcciones { get; set; } = new();
    }
}
