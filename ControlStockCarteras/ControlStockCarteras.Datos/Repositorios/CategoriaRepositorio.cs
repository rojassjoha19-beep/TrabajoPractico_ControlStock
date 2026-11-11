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
            return _contexto.Categorias
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToList();
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
        public Categoria? ObtenerPorId(int id)
        {
            return _contexto.Categorias.FirstOrDefault(c => c.Id == id && c.Activo);
        }

        public void Modificar(Categoria categoria)
        {
            var existente = _contexto.Categorias.Find(categoria.Id);
            if (existente != null)
            {
                existente.Nombre = categoria.Nombre;
                _contexto.SaveChanges();
            }
        }

        public void DarDeBaja(int id)
        {
            var categoria = _contexto.Categorias.Find(id);
            if (categoria != null)
            {
                categoria.Activo = false;
                _contexto.SaveChanges();
            }
        }
    }
}
