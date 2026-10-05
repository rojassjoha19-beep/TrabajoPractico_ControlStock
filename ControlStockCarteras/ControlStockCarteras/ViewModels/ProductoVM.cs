using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControlStockCarteras.ViewModels
{
    public class ProductoVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(50, ErrorMessage = "El color no puede superar los 50 caracteres.")]
        public string Color { get; set; } = "";

        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "No puede ser negativo.")]
        public int StockActual { get; set; }

        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "No puede ser negativo.")]
        public int StockMinimo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "No puede ser negativo.")]
        public int StockMaximo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(0.01, 100000000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seleccioná una categoría.")]
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public List<SelectListItem> Categorias { get; set; } = new();
    }
}