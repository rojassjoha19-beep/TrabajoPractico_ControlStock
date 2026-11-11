using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlStockCarteras.Datos.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaActivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Categorias",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Categorias");
        }
    }
}
