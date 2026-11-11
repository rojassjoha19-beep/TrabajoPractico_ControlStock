using ControlStockCarteras.Datos.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControlStockCarteras.ViewModels
{
    public class MovimientoVM
    {
        [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un producto.")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "Seleccioná el tipo de movimiento.")]
        [Display(Name = "Tipo de movimiento")]
        public TipoMovimiento? Tipo { get; set; }

        [Required(ErrorMessage = "Ingresá la cantidad.")]
        [Display(Name = "Cantidad")]
        public int? Cantidad { get; set; }

        public List<SelectListItem> Productos { get; set; } = new();
    }
}
