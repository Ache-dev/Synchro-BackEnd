# Synchro BackEnd

Backend ASP.NET Core (.NET 10) para consumir una base de datos existente con Dapper.

## Estado actual

- API REST
- Swagger habilitado en desarrollo
- SignalR disponible en `/chatHub`
- CORS configurado para orígenes de frontend locales
- Acceso a base de datos centralizado en `Repository.cs`
- Consultas SQL centralizadas en `Query.cs`
- Modelos de dominio en `Models.cs`
- Sin uso de DTOs

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Dapper
- Microsoft.Data.SqlClient
- Swashbuckle.AspNetCore
- SignalR

## Estructura principal

```text
ApiSynchro/
├─ Controllers/
├─ Hubs/
├─ Models.cs
├─ Query.cs
├─ Repository.cs
├─ Program.cs
└─ appsettings.json
```

## Configuración

### Connection string

Editar `ApiSynchro/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=SynchroDB;User Id=sa;Password=YourStrong@Password2026;TrustServerCertificate=True;"
  }
}
```

### Ejecutar el proyecto

```bash
cd ApiSynchro
dotnet run
```

## Swagger

En entorno de desarrollo, la documentación está disponible en la URL que expone la aplicación al iniciar.

## Endpoints disponibles

### Usuarios

- `GET /api/v1/usuarios`
- `GET /api/v1/usuarios/{id}`
- `POST /api/v1/usuarios/registro`
- `POST /api/v1/usuarios/login`
- `POST /api/v1/usuarios/logout`
- `PUT /api/v1/usuarios/{id}`
- `DELETE /api/v1/usuarios/{id}`
- `POST /api/v1/usuarios/{id}/generar-bio`

#### Login

- Entrada: JSON con `email` y `contrasena`
- Respuesta: JSON con `token`, `expiraEn` y `usuario`
- El token de cierre de sesión se envía en el encabezado `Authorization`
- Formato aceptado: `Bearer {token}` o token directo

### Matches

- `GET /api/v1/matches`
- `GET /api/v1/matches/usuario/{idUsuario}`
- `GET /api/v1/matches/{id}`
- `POST /api/v1/matches`
- `PUT /api/v1/matches/{id}/estado?estado=true|false`
- `DELETE /api/v1/matches/{id}`

### Mensajes

- `GET /api/v1/mensajes/match/{idMatch}`
- `GET /api/v1/mensajes/conversacion/{idRemitente}/{idDestinatario}`
- `GET /api/v1/mensajes/{id}`
- `GET /api/v1/mensajes/no-leidos/{idUsuario}`
- `POST /api/v1/mensajes`
- `PUT /api/v1/mensajes/{id}/marcar-leido`
- `DELETE /api/v1/mensajes/{id}`

### Encuestas

#### Preguntas

- `GET /api/v1/encuestas`
- `GET /api/v1/encuestas/{id}`
- `POST /api/v1/encuestas`
- `PUT /api/v1/encuestas/{id}`
- `DELETE /api/v1/encuestas/{id}`

#### Respuestas

- `GET /api/v1/encuestas/respuestas`
- `GET /api/v1/encuestas/respuestas/{id}`
- `GET /api/v1/encuestas/usuario/{idUsuario}/respuestas`
- `POST /api/v1/encuestas/respuestas`
- `POST /api/v1/encuestas/respuestas/batch?idUsuario={idUsuario}`
- `PUT /api/v1/encuestas/respuestas/{id}`
- `DELETE /api/v1/encuestas/respuestas/{id}`

### Intenciones

- `GET /api/v1/intenciones`
- `GET /api/v1/intenciones/{id}`
- `POST /api/v1/intenciones`
- `PUT /api/v1/intenciones/{id}`
- `DELETE /api/v1/intenciones/{id}`

### SignalR

- `/chatHub`

## Notas

- La API trabaja directamente con entidades de `Models.cs`.
- `Repository.cs` centraliza el acceso a datos y el manejo de sesiones.
- `Query.cs` contiene las sentencias SQL.
- El login crea una sesión con expiración de 7 días y devuelve un token.
