using Microsoft.AspNetCore.Identity;

namespace ControlStockCarteras.Models
{
    public class MovimientoStock
    {   
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }
        public int TipoMovimientoId { get; set; }
        public TipoMovimiento? tipoMovimiento { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        public string UsuarioId { get; set; } = null!;
        public IdentityUser? Usuario { get; set; }


    }
}
