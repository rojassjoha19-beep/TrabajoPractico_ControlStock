using ControlStockCarteras.Datos.Modelos;
using ControlStockCarteras.Datos.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlStockCarteras.Controllers
{
    [Authorize(Roles = "Administrador")]
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
    }
}