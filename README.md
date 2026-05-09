# Synchro BackEnd

Backend de Synchro construido con ASP.NET Core y Dapper.

## Estado actual del proyecto (mayo 2026)

El repositorio mantiene dos líneas de trabajo:

1. Arquitectura activa (la que hoy ejecuta la API)
   - Solución: `ApiSynchro.slnx`
   - Proyectos incluidos: `ApiSynchro`, `Models`, `Query`, `Repository`
   - API REST operativa con controladores CRUD por entidad.
   - OpenAPI/Swagger habilitado en entorno Development.
   - Acceso a datos con Dapper y SQL Server.

2. Arquitectura por capas en transición
   - Carpetas presentes: `Synchro.API`, `Synchro.Application`, `Synchro.Domain`, `Synchro.Infrastructure`.
   - Contienen código base (DTOs, entidades, servicios y repositorios), pero actualmente no tienen `.csproj` ni están referenciadas en la solución principal.
   - Se consideran una migración en progreso y no forman parte del flujo de ejecución actual.

## Arquitectura activa

### Proyectos y responsabilidades

- `ApiSynchro` (net10.0)
  - Punto de entrada (`Program.cs`)
  - Configuración de DI, CORS, Swagger y Controllers
  - Expone endpoints HTTP

- `Models` (netstandard2.1)
  - Entidades del dominio persistido

- `Query` (netstandard2.1)
  - Interfaces e implementaciones de lectura/consulta con Dapper

- `Repository` (netstandard2.1)
  - Interfaces e implementaciones de acceso y persistencia con Dapper

### Estructura actual (resumen)

```text
Synchro-BackEnd/
├─ ApiSynchro.slnx
├─ README.md
├─ ApiSynchro/
│  ├─ ApiSynchro.csproj
│  ├─ Program.cs
│  ├─ appsettings.json
│  ├─ appsettings.Development.json
│  ├─ Controllers/
│  │  ├─ UsuariosController.cs
│  │  ├─ IntencionBusquedaController.cs
│  │  ├─ MatchController.cs
│  │  ├─ MensajeController.cs
│  │  ├─ PreguntaEncuestaController.cs
│  │  ├─ RespuestaEncuestaController.cs
│  │  └─ SesionController.cs
│  └─ Hubs/ (vacío por ahora)
├─ Models/
│  ├─ Models.csproj
│  ├─ Usuario.cs
│  ├─ IntencionBusqueda.cs
│  ├─ Match.cs
│  ├─ Mensaje.cs
│  ├─ PreguntaEncuesta.cs
│  ├─ RespuestaEncuesta.cs
│  └─ Sesion.cs
├─ Query/
│  ├─ Query.csproj
│  ├─ Interfaces/
│  └─ Implementations/
├─ Repository/
│  ├─ Repository.csproj
│  ├─ Interfaces/
│  └─ Implements/
├─ Synchro.API/
├─ Synchro.Application/
├─ Synchro.Domain/
└─ Synchro.Infrastructure/
```

## Configuración

### Requisitos

- SDK de .NET 10
- SQL Server accesible desde la cadena de conexión

### Cadena de conexión (arquitectura activa)

Editar `ApiSynchro/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "sql": "Server=localhost,1433;Database=SynchroDB;User Id=sa;Password=TuPasswordSeguro123!;TrustServerCertificate=True;"
  }
}
```

Nota: `Program.cs` de `ApiSynchro` usa la clave `sql`.

### Ejecución local

```bash
cd ApiSynchro
dotnet run
```

Perfiles configurados en `launchSettings.json`:

- HTTP: `http://localhost:5124`
- HTTPS: `https://localhost:7274` (y también HTTP 5124)

## Endpoints actuales

La API no está versionada por ruta (`/api/v1` no aplica en la implementación actual).

Rutas base por controlador:

- `api/usuario`
- `api/intencionbusqueda`
- `api/match`
- `api/mensaje`
- `api/preguntaencuesta`
- `api/respuestaencuesta`
- `api/sesion`

Cada controlador expone operaciones CRUD base:

- `GET /api/{controller}`
- `GET /api/{controller}/{id}`
- `POST /api/{controller}`
- `PUT /api/{controller}/{id}`
- `DELETE /api/{controller}/{id}`

## Swagger y documentación

- En Development, Swagger UI se habilita con la configuración por defecto de ASP.NET Core (ruta `/swagger`).
- Se incluyen comentarios XML (`api.xml`) para documentar endpoints.

## Estado de componentes técnicos

- DI configurada para Queries y Repositories por entidad.
- CORS habilitado con política por defecto para `http://127.0.0.1:5500`.
- SignalR registrado (`AddSignalR()`), pero actualmente no hay Hub implementado ni mapeado en rutas.

## Pendientes técnicos identificados

- Integrar o formalizar la arquitectura por capas (`Synchro.*`) en proyectos compilables.
- Definir estrategia de versionado de API (si se requiere `v1`).
- Unificar criterios de cadenas de conexión entre arquitectura activa y estructura en transición.
- Ajustar CORS según el entorno real del frontend (por ejemplo, Vite en `:5173` si corresponde).
