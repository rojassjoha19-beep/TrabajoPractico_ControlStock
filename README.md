 Control de Stock - Carteras
Funcionalidad:
- Productos: listado, detalle, alta, edición y baja lógica.
- Categorías: listado, detalle, alta, edición y baja lógica.
- Movimientos de stock: registro de entradas, salidas y ajustes, con listado y el usuario que los realizó.
- Autenticación con Google y roles (Administrador y Empleado).

Estructura de la solución
-ControlStockCarteras: capa web (controladores, ViewModels, vistas y autenticación).
-ControlStockCarteras.Datos: capa de acceso a datos (modelos, contexto de Entity Framework, repositorios y migraciones).

Cómo ejecutarlo

1. Base de datos
Restaurar el archivoControlStockCarteras.bak incluido en este repositorio en SQL Server (LocalDB o instancia local):
clic derecho en "Bases de datos" > "Restaurar base de datos" > "Dispositivo" y elegir el archivo .bak.

Revisar que la cadena de conexión de appsettings.json apunte al servidor y al nombre de la base restaurada.

2. Claves de Google
clic derecho en el proyecto web > "Administrar secretos de usuario":

{
  "GoogleKeys": {
    "ClientId": "<tu client id>",
    "ClientSecret": "<tu client secret>"
  }
}

La URI de redireccionamiento autorizada en Google Cloud es
`https://localhost:7019/signin-google`

Mientras la aplicación de Google esté en modo de prueba, solo pueden ingresar los correos agregados como usuarios de prueba.

3. Ejecutar

Abrir la solución en Visual Studio y ejecutar el proyecto web.

Roles y permisos
- Administrador: todo (productos, categorías y movimientos: alta, edición y baja).
- Empleado: ver productos y categorías, y registrar movimientos de stock.

Gestión de roles

Asignar rol Administrador (RolId = 1)
```sql
INSERT INTO UsuariosRoles (UsuarioId, RolId)
SELECT Id, 1 FROM Usuarios WHERE Email = 'correo@gmail.com';
```

Cambiar de Administrador a Empleado (RolId = 2)
```sql
UPDATE UsuariosRoles SET RolId = 2
WHERE UsuarioId = (SELECT Id FROM Usuarios WHERE Email = 'correo@gmail.com');
```

Quitar el rol Administrador
```sql
DELETE FROM UsuariosRoles
WHERE RolId = 1 AND UsuarioId = (SELECT Id FROM Usuarios WHERE Email = 'correo@gmail.com');
```
El usuario que ingresa por primera vez es empleado automaticamente.
El usuario se registra en la tabla Usuarios la primera vez que inicia sesión con Google. Después de cambiar un rol, el usuario tiene que cerrar sesión y volver a ingresar.
