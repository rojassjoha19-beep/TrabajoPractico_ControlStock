using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlStockCarteras.Datos.Migrations
{
    /// <inheritdoc />
    public partial class TipoEnMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoMovimiento",
                table: "MovimientosStock");

            migrationBuilder.RenameColumn(
                name: "TipoMovimientoId",
                table: "MovimientosStock",
                newName: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "MovimientosStock",
                newName: "TipoMovimientoId");

            migrationBuilder.AddColumn<int>(
                name: "TipoMovimiento",
                table: "MovimientosStock",
                type: "int",
                nullable: true);
        }
    }
}
