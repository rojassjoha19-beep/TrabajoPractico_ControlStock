using System;
using System.Collections.Generic;
using System.Text;

// RolAccion.cs
namespace ControlStockCarteras.Datos.Modelos
{
    public class RolAccion
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
        public int AccionId { get; set; }
        public Accion? Accion { get; set; }
    }
}