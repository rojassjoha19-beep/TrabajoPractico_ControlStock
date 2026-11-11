using ControlStockCarteras.Datos.Modelos;
using ControlStockCarteras.Datos.Repositorios;
using ControlStockCarteras.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControlStockCarteras.Controllers
{
    [Authorize]
    public class MovimientosController : Controller
    {
        private readonly IMovimientoRepositorio _movimientos;
        private readonly IProductoRepositorio _productos;

        public MovimientosController(IMovimientoRepositorio movimientos, IProductoRepositorio productos)
        {
            _movimientos = movimientos;
            _productos = productos;
        }

        public IActionResult Index()
        {
            return View(_movimientos.ObtenerTodos());
        }

        public IActionResult Registrar()
        {
            var vm = new MovimientoVM();
            CargarProductos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrar(MovimientoVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var movimiento = new MovimientoStock
                    {
                        ProductoId = vm.ProductoId,
                        Tipo = vm.Tipo!.Value,
                        Cantidad = vm.Cantidad!.Value,
                        UsuarioId = User.FindFirst("IdUsuario")?.Value ?? ""
                    };
                    _movimientos.Registrar(movimiento);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "No se pudo registrar el movimiento. Intentá nuevamente.");
                }
            }

            CargarProductos(vm);
            return View(vm);
        }

        private void CargarProductos(MovimientoVM vm)
        {
            vm.Productos = _productos.ObtenerTodos()
                .Select(p => new SelectListItem 
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Nombre} - {p.Color} (stock: {p.StockActual})"
                })
                .ToList();
        }
    }
}