using ControlStockCarteras.Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Text;


namespace ControlStockCarteras.Datos.Repositorios
{
    public interface IProductoRepositorio
    {
        List<Producto> ObtenerTodos();
        Producto? ObtenerPorId(int id);
        void Agregar(Producto producto);
        void Modificar(Producto producto);
        void DarDeBaja(int id);
        bool ExisteDuplicado(int id, string nombre, string color);
    }
}
