using ControlStockCarteras.Data;
using ControlStockCarteras.Models;
using ControlStockCarteras.Repositories;
using ControlStockCarteras.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace ControlStockCarteras.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _repositorio;
        private readonly ApplicationDbContext _contexto;

        public ProductosController(IProductoRepository repositorio, ApplicationDbContext contexto)
        {
            _repositorio = repositorio;
            _contexto = contexto;
        }
        public async Task<IActionResult> Index()
        {
            var productos = await _contexto.Productos
                .Select(p => new ProductoVM
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Color = p.Color,
                    StockActual = p.StockActual,
                    StockMinimo = p.StockMinimo,
                    StockMaximo = p.StockMaximo,
                    Precio = p.Precio,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria!.Nombre
                })
                .ToListAsync();

            return View(productos);
        }

        public IActionResult Crear()
        {
            var vm = new ProductoVM { Categorias = ObtenerCategorias() };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(ProductoVM vm)
        {
            ValidarStock(vm);
            if (!ModelState.IsValid)
            {
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }
            if (ExisteDuplicado(0, vm.Nombre, vm.Color))
            {
                ModelState.AddModelError("", "Ya existe");
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }

            var producto = new Producto
            {
                Nombre = vm.Nombre,
                Color = vm.Color,
                StockActual = vm.StockActual,
                StockMinimo = vm.StockMinimo,
                StockMaximo = vm.StockMaximo,
                Precio = vm.Precio,
                CategoriaId = vm.CategoriaId
            };

            bool resultado = _repositorio.AgregarProducto(producto);
            if (!resultado)
            {
                ModelState.AddModelError("", "No se puede guardar el producto");
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

      
        public IActionResult Editar(int id)
        {
            var producto = _repositorio.ObtenerProductoPorId(id);
            if (producto == null) return NotFound();

            var vm = new ProductoVM
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Color = producto.Color,
                StockActual = producto.StockActual,
                StockMinimo = producto.StockMinimo,
                StockMaximo = producto.StockMaximo,
                Precio = producto.Precio,
                CategoriaId = producto.CategoriaId,
                Categorias = ObtenerCategorias()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ProductoVM vm)
        {
            ValidarStock(vm);
            if (!ModelState.IsValid)
            {
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }
            if (ExisteDuplicado(vm.Id, vm.Nombre, vm.Color))
            {
                ModelState.AddModelError("", "Ya existe ese producto");
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }
            var producto = new Producto
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Color = vm.Color,
                StockActual = vm.StockActual,
                StockMinimo = vm.StockMinimo,
                StockMaximo = vm.StockMaximo,
                Precio = vm.Precio,
                CategoriaId = vm.CategoriaId
            };

            bool resultado = _repositorio.ActualizarProducto(producto);
            if (!resultado)
            {
                ModelState.AddModelError("", "No se pudo actualizar el producto. Intentá nuevamente.");
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

      
        public IActionResult Eliminar(int id)
        {
            var producto = _repositorio.ObtenerProductoPorId(id);
            if (producto == null) return NotFound();

            var vm = new ProductoVM
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Color = producto.Color,
                StockActual = producto.StockActual,
                StockMinimo = producto.StockMinimo,
                StockMaximo = producto.StockMaximo,
                Precio = producto.Precio,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = _contexto.Categorias
                    .Where(c => c.Id == producto.CategoriaId)
                    .Select(c => c.Nombre)
                    .FirstOrDefault()
            };

            return View(vm);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            _repositorio.BorrarProducto(id);
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> ObtenerCategorias()
        {
            return _contexto.Categorias
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                .ToList();
        }
        private bool ExisteDuplicado(int id, string nombre, string color)
        {
            nombre = nombre.Trim();
            color = color.Trim();
            return _contexto.Productos
                .Any(p => p.Id != id && p.Nombre == nombre && p.Color == color);
        }
        private void ValidarStock(ProductoVM vm)
        {
            if (vm.StockMinimo > vm.StockMaximo)
            {
                ModelState.AddModelError("StockMinimo", "NO puede ser mayor al stock máximo.");
            }

            if (vm.StockActual < vm.StockMinimo)
            {
                ModelState.AddModelError("StockActual", "NO puede ser menor al stock mínimo.");
            }

            if (vm.StockActual > vm.StockMaximo)
            {
                ModelState.AddModelError("StockActual", "NO puede ser mayor al stock máximo.");
            }
        }
    }
}