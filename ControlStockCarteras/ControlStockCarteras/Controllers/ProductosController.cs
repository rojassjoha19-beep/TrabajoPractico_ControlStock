using ControlStockCarteras.Datos.Modelos;
using ControlStockCarteras.Datos.Repositorios;
using ControlStockCarteras.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControlStockCarteras.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductoRepositorio _productos;
        private readonly ICategoriaRepositorio _categorias;

        public ProductosController(IProductoRepositorio productos, ICategoriaRepositorio categorias)
        {
            _productos = productos;
            _categorias = categorias;
        }

        public IActionResult Index()
        {
            var lista = _productos.ObtenerTodos()
                .Select(p => ArmarVM(p))
                .ToList();
            return View(lista);
        }

        public IActionResult Detalle(int id)
        {
            var producto = _productos.ObtenerPorId(id);
            if (producto == null || !producto.Activo) return NotFound();
            return View(ArmarVM(producto));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            var vm = new ProductoVM { Categorias = ObtenerCategorias() };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(ProductoVM vm)
        {
            ValidarStock(vm);
            if (_productos.ExisteDuplicado(0, vm.Nombre, vm.Color))
            {
                ModelState.AddModelError("", "Ya existe un producto con ese nombre y color.");
            }
            if (!ModelState.IsValid)
            {
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }

            try
            {
                _productos.Agregar(ArmarProducto(vm));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo guardar el producto. Intentá nuevamente.");
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }
        }
        
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var producto = _productos.ObtenerPorId(id);
            if (producto == null || !producto.Activo) return NotFound();

            var vm = ArmarVM(producto);
            vm.Categorias = ObtenerCategorias();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(ProductoVM vm)
        {
            ValidarStock(vm);
            if (_productos.ExisteDuplicado(vm.Id, vm.Nombre, vm.Color))
            {
                ModelState.AddModelError("", "Ya existe un producto con ese nombre y color.");
            }
            if (!ModelState.IsValid)
            {
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }

            try
            {
                _productos.Modificar(ArmarProducto(vm));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo actualizar el producto. Intentá nuevamente.");
                vm.Categorias = ObtenerCategorias();
                return View(vm);
            }
        }
        
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            var producto = _productos.ObtenerPorId(id);
            if (producto == null || !producto.Activo) return NotFound();
            return View(ArmarVM(producto));
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
            _productos.DarDeBaja(id);
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> ObtenerCategorias()
        {
            return _categorias.ObtenerTodas()
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                .ToList();
        }

        private static ProductoVM ArmarVM(Producto p)
        {
            return new ProductoVM
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Color = p.Color,
                StockActual = p.StockActual,
                StockMinimo = p.StockMinimo,
                StockMaximo = p.StockMaximo,
                Precio = p.Precio,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre ?? ""
            };
        }

        private static Producto ArmarProducto(ProductoVM vm)
        {
            return new Producto
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
        }

        private void ValidarStock(ProductoVM vm)
        {
            if (vm.StockMinimo > vm.StockMaximo)
            {
                ModelState.AddModelError("StockMinimo", "No puede ser mayor al stock máximo.");
            }
            if (vm.StockActual < vm.StockMinimo)
            {
                ModelState.AddModelError("StockActual", "No puede ser menor al stock mínimo.");
            }
            if (vm.StockActual > vm.StockMaximo)
            {
                ModelState.AddModelError("StockActual", "No puede ser mayor al stock máximo.");
            }
        }
    }
}