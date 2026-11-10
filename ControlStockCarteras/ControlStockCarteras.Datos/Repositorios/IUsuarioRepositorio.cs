using ControlStockCarteras.Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControlStockCarteras.Datos.Repositorios
{
    public interface IUsuarioRepositorio
    {
        Usuario? ObtenerOCrearUsuario(string email, string nombre);
        List<string> ObtenerAcciones(string email);
        List<string> ObtenerRoles(string email);
    }
}
