using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace ControlStockCarteras.Datos.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Email { get; set; } = "";

        [StringLength(100)]
        public string Nombre { get; set; } = "";

        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public List<UsuarioRol> UsuariosRoles { get; set; } = new();
    }
}