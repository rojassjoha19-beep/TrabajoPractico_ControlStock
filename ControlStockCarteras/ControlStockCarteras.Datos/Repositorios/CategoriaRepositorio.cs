using ControlStockCarteras.Datos.Contexto;
using ControlStockCarteras.Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Text;


namespace ControlStockCarteras.Datos.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ContextoStock _contexto;

        public CategoriaRepositorio(ContextoStock contexto)
        {
            _contexto = contexto;
        }

        public List<Categoria> ObtenerTodas()
        {
            return _contexto.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public void Agregar(Categoria categoria)
        {
            _contexto.Categorias.Add(categoria);
            _contexto.SaveChanges();
        }

        public bool ExisteDuplicado(int id, string nombre)
        {
            nombre = nombre.Trim();
            return _contexto.Categorias
                .Any(c => c.Id != id && c.Nombre == nombre);
        }
    }
}
