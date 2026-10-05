using ControlStockCarteras.Data;
using ControlStockCarteras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlStockCarteras.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return View(categorias);
        }

        public IActionResult Crear()
        {
            return View(new Categoria());
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (ExisteDuplicado(0, categoria.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.Nombre = categoria.Nombre.Trim();

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
            private bool ExisteDuplicado(int id, string nombre)
        {
            nombre = nombre.Trim();

            return _context.Categorias
                .Any(c => c.Id != id && c.Nombre == nombre);
        }
    }
}