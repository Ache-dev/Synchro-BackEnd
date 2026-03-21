# API Synchro

API completa para la aplicación de matching SynchroDB con SignalR para chat en tiempo real.

## Características

- ✅ Gestión completa de usuarios (registro, login, actualización, eliminación)
- ✅ Sistema de sesiones con tokens
- ✅ Sistema de encuestas con preguntas y respuestas
- ✅ Sistema de matches con compatibilidad e IA
- ✅ Chat en tiempo real con SignalR
- ✅ Historial de mensajes
- ✅ Notificaciones de mensajes no leídos
- ✅ Indicador de "usuario escribiendo"

## Tecnologías

- .NET 10
- Entity Framework Core 10
- SQL Server
- SignalR
- BCrypt para encriptación de contraseñas
- Swagger para documentación

## Configuración

### 1. Base de Datos

Asegúrate de tener SQL Server instalado y ejecuta el script SQL proporcionado para crear la base de datos `SynchroDB`.

### 2. Cadena de Conexión

Actualiza la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=SynchroDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Restaurar Paquetes

```bash
dotnet restore
```

### 4. Ejecutar la API

```bash
dotnet run
```

La API estará disponible en: `https://localhost:7xxx` y `http://localhost:5xxx`

## Endpoints

### Usuarios

- `POST /api/usuarios/registro` - Registrar nuevo usuario
- `POST /api/usuarios/login` - Iniciar sesión
- `POST /api/usuarios/logout` - Cerrar sesión
- `GET /api/usuarios` - Obtener todos los usuarios
- `GET /api/usuarios/{id}` - Obtener usuario por ID
- `PUT /api/usuarios/{id}` - Actualizar usuario
- `DELETE /api/usuarios/{id}` - Eliminar usuario (soft delete)

### Matches

- `POST /api/matches?idUsuario1={id}` - Crear match
- `GET /api/matches/usuario/{idUsuario}` - Obtener matches de un usuario
- `GET /api/matches/{id}` - Obtener match por ID
- `PUT /api/matches/{id}` - Actualizar match
- `DELETE /api/matches/{id}` - Eliminar match

### Mensajes

- `POST /api/mensajes?idRemitente={id}` - Enviar mensaje
- `GET /api/mensajes/match/{idMatch}` - Obtener mensajes de un match
- `GET /api/mensajes/no-leidos/{idUsuario}` - Obtener mensajes no leídos
- `PUT /api/mensajes/{id}/leer` - Marcar mensaje como leído
- `DELETE /api/mensajes/{id}` - Eliminar mensaje

### Encuestas

- `GET /api/encuestas/preguntas` - Obtener todas las preguntas
- `POST /api/encuestas/preguntas` - Crear pregunta
- `POST /api/encuestas/respuestas?idUsuario={id}` - Guardar respuestas
- `GET /api/encuestas/respuestas/usuario/{idUsuario}` - Obtener respuestas de un usuario

## SignalR - Chat Hub

### Conexión

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7xxx/chatHub")
    .build();

await connection.start();
```

### Métodos del Cliente (Enviar al servidor)

#### ConectarUsuario
```javascript
await connection.invoke("ConectarUsuario", idUsuario);
```

#### EnviarMensaje
```javascript
const mensaje = {
    idMatch: 1,
    idDestinatario: 2,
    mensajeTexto: "Hola!",
    tipoMensaje: "texto"
};

await connection.invoke("EnviarMensaje", idRemitente, mensaje);
```

#### MarcarComoLeido
```javascript
await connection.invoke("MarcarComoLeido", idMensaje);
```

#### NotificarEscribiendo
```javascript
await connection.invoke("NotificarEscribiendo", idUsuarioReceptor, idMatch, true);
```

#### ObtenerHistorialChat
```javascript
const mensajes = await connection.invoke("ObtenerHistorialChat", idMatch);
```

#### ObtenerMensajesNoLeidos
```javascript
const mensajes = await connection.invoke("ObtenerMensajesNoLeidos", idUsuario);
```

### Métodos del Servidor (Recibir del servidor)

#### UsuarioConectado
```javascript
connection.on("UsuarioConectado", (mensaje) => {
    console.log(mensaje);
});
```

#### RecibirMensaje
```javascript
connection.on("RecibirMensaje", (mensaje) => {
    console.log("Nuevo mensaje:", mensaje);
    // Actualizar UI con el nuevo mensaje
});
```

#### MensajeEnviado
```javascript
connection.on("MensajeEnviado", (mensaje) => {
    console.log("Mensaje enviado correctamente:", mensaje);
});
```

#### MensajeLeido
```javascript
connection.on("MensajeLeido", (idMensaje) => {
    console.log("Mensaje leído:", idMensaje);
});
```

#### UsuarioEscribiendo
```javascript
connection.on("UsuarioEscribiendo", (idMatch, escribiendo) => {
    if (escribiendo) {
        console.log("El usuario está escribiendo...");
    } else {
        console.log("El usuario dejó de escribir");
    }
});
```

## Ejemplo de Uso Completo

### 1. Registrar Usuario

```http
POST /api/usuarios/registro
Content-Type: application/json

{
  "nombre": "Juan Pérez",
  "email": "juan@example.com",
  "contrasena": "miPassword123",
  "fechaNacimiento": "1995-05-15",
  "ciudad": "Madrid",
  "genero": "Masculino",
  "idiomaPreferido": "es"
}
```

### 2. Login

```http
POST /api/usuarios/login
Content-Type: application/json

{
  "email": "juan@example.com",
  "contrasena": "miPassword123"
}
```

Respuesta:
```json
{
  "token": "abc123xyz...",
  "expiraEn": "2024-01-20T10:00:00",
  "usuario": {
    "idUsuario": 1,
    "nombre": "Juan Pérez",
    "email": "juan@example.com",
    ...
  }
}
```

### 3. Crear Match

```http
POST /api/matches?idUsuario1=1
Content-Type: application/json

{
  "idUsuario2": 2,
  "compatibilidad": 85.5,
  "explicacionAfinidad": "Ambos disfrutan actividades al aire libre",
  "sugerenciaIA": "Podrían planear una excursión juntos"
}
```

### 4. Conectar al Chat y Enviar Mensaje

```javascript
// Conectar
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7xxx/chatHub")
    .build();

await connection.start();

// Registrar usuario en el hub
await connection.invoke("ConectarUsuario", 1);

// Escuchar mensajes entrantes
connection.on("RecibirMensaje", (mensaje) => {
    console.log("Nuevo mensaje:", mensaje);
});

// Enviar mensaje
const mensaje = {
    idMatch: 1,
    idDestinatario: 2,
    mensajeTexto: "Hola! ¿Cómo estás?",
    tipoMensaje: "texto"
};

await connection.invoke("EnviarMensaje", 1, mensaje);
```

## Estructura del Proyecto

```
ApiSynchro/
├── Controllers/
│   ├── UsuariosController.cs
│   ├── MatchesController.cs
│   ├── MensajesController.cs
│   └── EncuestasController.cs
├── Data/
│   └── SynchroDbContext.cs
├── DTOs/
│   ├── UsuarioDTOs.cs
│   ├── MatchDTOs.cs
│   ├── MensajeDTOs.cs
│   ├── PreguntaEncuestaDTOs.cs
│   └── RespuestaEncuestaDTOs.cs
├── Hubs/
│   └── ChatHub.cs
├── Models/
│   ├── Usuario.cs
│   ├── Sesion.cs
│   ├── Match.cs
│   ├── Mensaje.cs
│   ├── PreguntaEncuesta.cs
│   └── RespuestaEncuesta.cs
├── Services/
│   ├── UsuarioService.cs
│   ├── MatchService.cs
│   ├── MensajeService.cs
│   └── EncuestaService.cs
├── Program.cs
└── appsettings.json
```

## Seguridad

- Las contraseñas se encriptan usando BCrypt
- Sistema de tokens para sesiones
- Validación de tokens en cada request
- CORS configurado para desarrollo

## Notas

- Asegúrate de tener la base de datos `SynchroDB` creada antes de ejecutar la API
- La cadena de conexión usa autenticación de Windows por defecto
- Para usar autenticación SQL Server, actualiza la cadena de conexión:
  ```
  "Server=localhost;Database=SynchroDB;User Id=tu_usuario;Password=tu_password;TrustServerCertificate=True;MultipleActiveResultSets=true"
  ```

## Swagger

Una vez iniciada la API, accede a la documentación interactiva en:
- `https://localhost:7xxx/swagger`

## Soporte

Para problemas o preguntas, crea un issue en el repositorio.
