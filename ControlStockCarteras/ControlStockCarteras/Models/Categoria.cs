using System.ComponentModel.DataAnnotations;

namespace ControlStockCarteras.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; } = "";

        public List<Producto> Productos { get; set; } = new();
    }
}