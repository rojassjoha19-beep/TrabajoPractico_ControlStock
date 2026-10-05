using ControlStockCarteras.Data;
using ControlStockCarteras.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlStockCarteras.Repositories
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerProductos();
        Producto ObtenerProductoPorId(int idProducto);
        bool AgregarProducto(Producto producto);
        bool ActualizarProducto(Producto producto);
        bool BorrarProducto(int idProducto);
    }

    public class ProductoRepository : IProductoRepository
    {
        private ApplicationDbContext _db;

        public ProductoRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public List<Producto> ObtenerProductos()
        {
            return _db.Productos.Include(p => p.Categoria).ToList();
        }

        public Producto ObtenerProductoPorId(int idProducto)
        {
            Producto producto = _db.Productos.Include(p => p.Categoria)
                                              .FirstOrDefault(p => p.Id == idProducto);
            return producto;
        }

        public bool AgregarProducto(Producto producto)
        {
            try
            {
                _db.Productos.Add(producto);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return false;
            }
        }

        public bool ActualizarProducto(Producto producto)
        {
            try
            {
                _db.Productos.Update(producto);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return false;
            }
        }

        public bool BorrarProducto(int idProducto)
        {
            try
            {
                Producto producto = _db.Productos.Find(idProducto);
                if (producto != null)
                {
                    _db.Productos.Remove(producto);
                    _db.SaveChanges();
                    return true;
                }
                return false; // Producto no encontrado
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return false;
            }
        }
    }
}
