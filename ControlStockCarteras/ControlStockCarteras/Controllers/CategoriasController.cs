using ControlStockCarteras.Datos.Modelos;
using ControlStockCarteras.Datos.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlStockCarteras.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepositorio _categorias;

        public CategoriasController(ICategoriaRepositorio categorias)
        {
            _categorias = categorias;
        }

        public IActionResult Index()
        {
            return View(_categorias.ObtenerTodas());
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            return View(new Categoria());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(Categoria categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria.Nombre)
                && _categorias.ExisteDuplicado(0, categoria.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            try
            {
                categoria.Nombre = categoria.Nombre.Trim();
                _categorias.Agregar(categoria);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo guardar la categoría. Intentá nuevamente.");
                return View(categoria);
            }
        }
        public IActionResult Detalle(int id)
        {
            var categoria = _categorias.ObtenerPorId(id);
            if (categoria == null)
                return NotFound();
            return View(categoria);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var categoria = _categorias.ObtenerPorId(id);
            if (categoria == null)
                return NotFound();
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(Categoria categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria.Nombre)
                && _categorias.ExisteDuplicado(categoria.Id, categoria.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
            }

            if (!ModelState.IsValid)
                return View(categoria);

            try
            {
                categoria.Nombre = categoria.Nombre.Trim();
                _categorias.Modificar(categoria);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se puede guardar la categoría.");
                return View(categoria);
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            var categoria = _categorias.ObtenerPorId(id);
            if (categoria == null)
                return NotFound();
            return View(categoria);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                _categorias.DarDeBaja(id);
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo dar de baja la categoría.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}