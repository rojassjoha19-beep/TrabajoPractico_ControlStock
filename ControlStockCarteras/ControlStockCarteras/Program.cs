using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using ControlStockCarteras.Datos.Contexto;
using ControlStockCarteras.Datos.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cadenaConexion = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<ContextoStock>(opciones =>
    opciones.UseSqlServer(cadenaConexion));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(opciones =>
{
    opciones.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opciones.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(opciones =>
{
    opciones.AccessDeniedPath = "/Sesion/AccesoDenegado";
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, opciones =>
{
    opciones.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value!;
    opciones.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value!;
});
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IMovimientoRepositorio, MovimientoRepositorio>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();