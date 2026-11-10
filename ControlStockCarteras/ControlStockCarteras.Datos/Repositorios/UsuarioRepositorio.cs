using System;
using System.Collections.Generic;
using System.Text;

// UsuarioRepositorio.cs
using ControlStockCarteras.Datos.Contexto;
using ControlStockCarteras.Datos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ControlStockCarteras.Datos.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ContextoStock _contexto;

        public UsuarioRepositorio(ContextoStock contexto)
        {
            _contexto = contexto;
        }
    
        public Usuario? ObtenerOCrearUsuario(string email, string nombre)
        {
            try
            {
                var usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == email);
                if (usuario != null)
                {
                    return usuario;
                }

                usuario = new Usuario { Email = email, Nombre = nombre, FechaAlta = DateTime.Now };
                _contexto.Usuarios.Add(usuario);
                _contexto.SaveChanges();
                return usuario;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> ObtenerAcciones(string email)
        {
            try
            {
                var usuario = _contexto.Usuarios
                    .Include(u => u.UsuariosRoles)
                        .ThenInclude(ur => ur.Rol)
                        .ThenInclude(r => r!.RolesAcciones)
                        .ThenInclude(ra => ra.Accion)
                    .FirstOrDefault(u => u.Email == email);

                if (usuario == null)
                {
                    return new List<string>();
                }

                return usuario.UsuariosRoles
                    .SelectMany(ur => ur.Rol!.RolesAcciones)
                    .Select(ra => ra.Accion!.Descripcion)
                    .Distinct()
                    .ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
        public List<string> ObtenerRoles(string email)
        {
            try
            {
                return _contexto.UsuariosRoles
                    .Where(ur => ur.Usuario!.Email == email)
                    .Select(ur => ur.Rol!.Nombre)
                    .ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}