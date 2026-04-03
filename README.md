# Synchro BackEnd

<<<<<<< HEAD
Backend ASP.NET Core (.NET 10) para consumo de base de datos con Dapper.
=======
Backend ASP.NET Core (.NET 10) orientado al consumo de una base de datos existente mediante Dapper.
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62

## Estado actual

- API REST en `http://localhost:5000`
<<<<<<< HEAD
- Swagger habilitado en desarrollo en la raíz
- SignalR habilitado en `/chatHub`
- CORS abierto para cualquier origen, encabezado y método
- Acceso a BD centralizado en `Repository.cs`
- Consultas SQL centralizadas en `Query.cs`
- Modelos de dominio en `Models.cs`
- Sin capa de DTOs
- Sin carpeta `Services`
=======
- Swagger habilitado en desarrollo
- SignalR habilitado en `/chatHub`
- Acceso a BD con una clase central `Repository`
- Consultas SQL centralizadas en `Query.cs`
- Sin DTOs ni interfaces
- Sin carpeta `Data`, `Repository` ni `Security`
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Dapper
<<<<<<< HEAD
- Dapper.Contrib
=======
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62
- Microsoft.Data.SqlClient
- Swashbuckle.AspNetCore
- SignalR

## Estructura principal

```text
ApiSynchro/
<<<<<<< HEAD
├─ Controllers/
├─ Hubs/
├─ AppHost.cs
├─ Models.cs
├─ Query.cs
├─ Repository.cs
└─ appsettings.json
=======
+- Controllers/
+- Hubs/
+- Models.cs
+- Program.cs
+- Query.cs
+- Repository.cs
+- appsettings.json
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62
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

<<<<<<< HEAD
En desarrollo abre:
=======
En entorno de desarrollo abre:
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62

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

<<<<<<< HEAD
#### Login

- Entrada: JSON con `email` y `contrasena`
- Respuesta: JSON con `token`, `expiraEn` y `usuario`
- El token de cierre de sesión se envía en el encabezado `Authorization`
- Formato aceptado: `Bearer {token}` o token directo

=======
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62
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
<<<<<<< HEAD
- `GET /api/v1/mensajes/no-leidos/{idUsuario}`
=======
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62
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
<<<<<<< HEAD
- `POST /api/v1/encuestas/respuestas/batch?idUsuario={idUsuario}`
=======
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62
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

<<<<<<< HEAD
- La API trabaja con entidades de `Models.cs`.
- `Repository.cs` centraliza acceso a datos y hash de contraseñas.
- `Query.cs` contiene las sentencias SQL.
- El login crea sesión con expiración de 7 días y devuelve token.
=======
- El backend trabaja directamente con entidades de `Models.cs`.
- `Repository.cs` centraliza el acceso a la base de datos.
- `Query.cs` contiene las sentencias SQL utilizadas por la API.
>>>>>>> 74febe41812b6cff4fdb8846a9a696e852b90f62
