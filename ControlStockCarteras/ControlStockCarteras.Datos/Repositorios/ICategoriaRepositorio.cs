using ControlStockCarteras.Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Text;




namespace ControlStockCarteras.Datos.Repositorios
{
    public interface ICategoriaRepositorio
    {
        List<Categoria> ObtenerTodas();
        void Agregar(Categoria categoria);
        bool ExisteDuplicado(int id, string nombre);
        Categoria? ObtenerPorId(int id);
        void Modificar(Categoria categoria);
        void DarDeBaja(int id);


    }
}
