using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ControlStockCarteras.Datos.Modelos
{
    public class Accion
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Descripcion { get; set; } = "";

        public List<RolAccion> RolesAcciones { get; set; } = new();
    }
}

