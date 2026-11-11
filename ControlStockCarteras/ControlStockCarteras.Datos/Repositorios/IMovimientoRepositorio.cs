using System;
using System.Collections.Generic;
using System.Text;

using ControlStockCarteras.Datos.Modelos;

namespace ControlStockCarteras.Datos.Repositorios
{
    public interface IMovimientoRepositorio
    {
        List<MovimientoStock> ObtenerTodos();
        void Registrar(MovimientoStock movimiento);
    }
}
