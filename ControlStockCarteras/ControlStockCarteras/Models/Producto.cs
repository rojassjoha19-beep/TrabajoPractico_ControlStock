using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlStockCarteras.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El color es obligatorio.")]
        [StringLength(50, ErrorMessage = "El color no puede superar los 50 caracteres.")]
        public string Color { get; set; } = "";

        [Required(ErrorMessage = "El stock actual es obligatorio.")]
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccioná una categoría.")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public List<MovimientoStock> Movimientos { get; set; } = new();
    }
}