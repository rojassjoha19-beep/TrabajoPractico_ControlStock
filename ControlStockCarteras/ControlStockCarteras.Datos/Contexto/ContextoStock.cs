
using ControlStockCarteras.Datos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ControlStockCarteras.Datos.Contexto
{
    public class ContextoStock : DbContext
    {
        public ContextoStock(DbContextOptions<ContextoStock> options)
     : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public DbSet<RolAccion> RolesAcciones { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MovimientoStock>()
                .HasOne(m => m.Producto)
                .WithMany(p => p.Movimientos)
                .HasForeignKey(m => m.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Producto>()
             .HasOne(p => p.Categoria)
             .WithMany(c => c.Productos)
             .HasForeignKey(p => p.CategoriaId)
             .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MovimientoStock>();
            builder.Entity<Usuario>()
    .HasIndex(u => u.Email)
    .IsUnique();

            builder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Empleado" });

            builder.Entity<Accion>().HasData(
                new Accion { Id = 1, Descripcion = "VerProductos" },
                new Accion { Id = 2, Descripcion = "AdministrarProductos" },
                new Accion { Id = 3, Descripcion = "AdministrarCategorias" },
                new Accion { Id = 4, Descripcion = "RegistrarMovimientos" });

            builder.Entity<RolAccion>().HasData(
                new RolAccion { Id = 1, RolId = 1, AccionId = 1 },
                new RolAccion { Id = 2, RolId = 1, AccionId = 2 },
                new RolAccion { Id = 3, RolId = 1, AccionId = 3 },
                new RolAccion { Id = 4, RolId = 1, AccionId = 4 },
                new RolAccion { Id = 5, RolId = 2, AccionId = 1 },
                new RolAccion { Id = 6, RolId = 2, AccionId = 4 });
        }

        
    }
}