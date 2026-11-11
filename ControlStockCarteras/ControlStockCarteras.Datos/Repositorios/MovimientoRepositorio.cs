using ControlStockCarteras.Datos.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ControlStockCarteras.Datos.Contexto;

namespace ControlStockCarteras.Datos.Repositorios
{
    public class MovimientoRepositorio : IMovimientoRepositorio
    {
        private readonly ContextoStock _contexto;

        public MovimientoRepositorio(ContextoStock contexto)
        {
            _contexto = contexto;
        }

        public List<MovimientoStock> ObtenerTodos()
        {
            return _contexto.MovimientosStock
                .Include(m => m.Producto)
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }

        public void Registrar(MovimientoStock movimiento)
        {
            var producto = _contexto.Productos.Find(movimiento.ProductoId);
            if (producto == null || !producto.Activo)
                throw new InvalidOperationException("El producto no existe o está dado de baja.");

            if (movimiento.Tipo != TipoMovimiento.Ajuste && movimiento.Cantidad <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            if (movimiento.Tipo == TipoMovimiento.Ajuste && movimiento.Cantidad == 0)
                throw new InvalidOperationException("El ajuste no puede ser cero.");

            int nuevoStock = movimiento.Tipo switch
            {
                TipoMovimiento.Entrada => producto.StockActual + movimiento.Cantidad,
                TipoMovimiento.Salida => producto.StockActual - movimiento.Cantidad,
                _ => producto.StockActual + movimiento.Cantidad  
            };

            if (nuevoStock < 0)
                throw new InvalidOperationException("No hay stock suficiente para este movimiento.");

            producto.StockActual = nuevoStock;
            movimiento.Fecha = DateTime.Now;
            _contexto.MovimientosStock.Add(movimiento);
            _contexto.SaveChanges();  
        }
    }
}
