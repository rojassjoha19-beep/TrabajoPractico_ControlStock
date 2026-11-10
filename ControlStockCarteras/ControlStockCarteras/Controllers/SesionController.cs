using ControlStockCarteras.Datos.Repositorios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControlStockCarteras.Controllers
{
    public class SesionController : Controller
    {
        private readonly IUsuarioRepositorio _usuarios;

        public SesionController(IUsuarioRepositorio usuarios)
        {
            _usuarios = usuarios;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task IniciarSesion()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = Url.Action("RespuestaGoogle") });
        }

        public async Task<IActionResult> RespuestaGoogle()
        {
            var resultado = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!resultado.Succeeded || resultado.Principal == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var email = resultado.Principal.FindFirstValue(ClaimTypes.Email);
            var nombre = resultado.Principal.FindFirstValue(ClaimTypes.Name) ?? "";
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(Index));
            }

            var usuario = _usuarios.ObtenerOCrearUsuario(email, nombre);
            if (usuario == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var claims = new List<Claim>(resultado.Principal.Claims)
            {
                new Claim("IdUsuario", usuario.Id.ToString())
            };

            foreach (var rol in _usuarios.ObtenerRoles(email))
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            foreach (var permiso in _usuarios.ObtenerAcciones(email))
            {
                claims.Add(new Claim("Permiso", permiso));
            }

            var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identidad));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}