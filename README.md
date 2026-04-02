# Synchro BackEnd

Backend ASP.NET Core (.NET 10) orientado al consumo de una base de datos existente mediante Dapper.

## Estado actual

- API REST en `http://localhost:5000`
- Swagger habilitado en desarrollo
- SignalR habilitado en `/chatHub`
- Acceso a BD con una clase central `Repository`
- Consultas SQL centralizadas en `Query.cs`
- Sin DTOs ni interfaces
- Sin carpeta `Data`, `Repository` ni `Security`

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
+- Controllers/
+- Hubs/
+- Models.cs
+- Program.cs
+- Query.cs
+- Repository.cs
+- appsettings.json
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

### Ejecutar

```bash
cd ApiSynchro
dotnet run
```

## Swagger

En entorno de desarrollo abre:

- `http://localhost:5000`

## Endpoints disponibles

### Usuarios

- `GET /api/v1/usuarios`
- `GET /api/v1/usuarios/{id}`
- `POST /api/v1/usuarios/registro`
- `POST /api/v1/usuarios/login`
- `POST /api/v1/usuarios/logout`
- `PUT /api/v1/usuarios/{id}`
- `DELETE /api/v1/usuarios/{id}`

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

- El backend trabaja directamente con entidades de `Models.cs`.
- `Repository.cs` centraliza el acceso a la base de datos.
- `Query.cs` contiene las sentencias SQL utilizadas por la API.
