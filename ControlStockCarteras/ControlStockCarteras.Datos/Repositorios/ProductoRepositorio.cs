// ProductoRepositorio.cs
using ControlStockCarteras.Datos.Contexto;
using ControlStockCarteras.Datos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ControlStockCarteras.Datos.Repositorios
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly ContextoStock _contexto;

        public ProductoRepositorio(ContextoStock contexto)
        {
            _contexto = contexto;
        }

        public List<Producto> ObtenerTodos()
        {
            return _contexto.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo)
                .ToList();
        }

        public Producto? ObtenerPorId(int id)
        {
            return _contexto.Productos
                .Include(p => p.Categoria)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Agregar(Producto producto)
        {
            _contexto.Productos.Add(producto);
            _contexto.SaveChanges();
        }

        public void Modificar(Producto producto)
        {
            var existente = _contexto.Productos.Find(producto.Id);
            if (existente == null)
            {
                return;
            }

            existente.Nombre = producto.Nombre;
            existente.Color = producto.Color;
            existente.Precio = producto.Precio;
            existente.StockMinimo = producto.StockMinimo;
            existente.StockMaximo = producto.StockMaximo;
            existente.CategoriaId = producto.CategoriaId;

            _contexto.SaveChanges();
        }

        public void DarDeBaja(int id)
        {
            var producto = _contexto.Productos.Find(id);
            if (producto == null)
            {
                return;
            }

            producto.Activo = false;
            _contexto.SaveChanges();
        }

        public bool ExisteDuplicado(int id, string nombre, string color)
        {
            nombre = nombre.Trim();
            color = color.Trim();
            return _contexto.Productos
                .Any(p => p.Activo && p.Id != id && p.Nombre == nombre && p.Color == color);
        }
    }
}
