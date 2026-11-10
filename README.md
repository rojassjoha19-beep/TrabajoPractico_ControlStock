# Control de Stock - Carteras

### Claves de Google
Las claves **no están en este repo**.Se guardan cn los secretos de usuario.
(clic derecho en el proyecto web > "Administrar secretos de usuario"):

```json
{
  "GoogleKeys": {
    "ClientId": "<tu client id>",
    "ClientSecret": "<tu client secret>"
  }
}
La URI de redireccionamiento autorizada en Google Cloud es
`https://localhost:<puerto>/signin-google`
## Gestión de roles

### Asignar rol Administrador (RolId = 1)
```sql
INSERT INTO UsuariosRoles (UsuarioId, RolId)
SELECT Id, 1 FROM Usuarios WHERE Email = 'correo@gmail.com';
```

### Cambiar de Administrador a Empleado (RolId = 2)
```sql
UPDATE UsuariosRoles SET RolId = 2
WHERE UsuarioId = (SELECT Id FROM Usuarios WHERE Email = 'correo@gmail.com');
```

### Quitar rol Administrador
```sql
DELETE FROM UsuariosRoles
WHERE RolId = 1 AND UsuarioId = (SELECT Id FROM Usuarios WHERE Email = 'correo@gmail.com');
```

Después de cambiar un rol, el usuario tiene que cerrar sesion y volver a ingresar.
