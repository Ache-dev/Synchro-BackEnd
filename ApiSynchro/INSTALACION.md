# 🚀 GUÍA DE INSTALACIÓN Y CONFIGURACIÓN - API SYNCHRO

## 📋 Prerrequisitos

Antes de comenzar, asegúrate de tener instalado:

- ✅ Visual Studio 2026 (o Visual Studio Code)
- ✅ .NET 10 SDK
- ✅ SQL Server (LocalDB, Express o Full)
- ✅ SQL Server Management Studio (SSMS) - Opcional pero recomendado

---

## 🗄️ PASO 1: Configurar la Base de Datos

### Opción A: Usando SQL Server Management Studio (SSMS)

1. Abre SSMS y conéctate a tu servidor SQL Server
2. Abre una nueva consulta (Ctrl + N)
3. Copia y pega el script SQL completo que se te proporcionó
4. Ejecuta el script (F5)
5. Verifica que se creó la base de datos `SynchroDB` con todas sus tablas

### Opción B: Usando el terminal/cmd

```bash
sqlcmd -S localhost -E -i script-database.sql
```

### Verificar la instalación

Ejecuta esta consulta para verificar:

```sql
USE SynchroDB
GO

SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
GO
```

Deberías ver 6 tablas:
- Usuario
- Sesion
- PreguntaEncuesta
- RespuestaEncuesta
- Match
- Mensaje

---

## ⚙️ PASO 2: Configurar la Cadena de Conexión

1. Abre el archivo `appsettings.json`

2. Actualiza la cadena de conexión según tu configuración:

### Para SQL Server con autenticación de Windows:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SynchroDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Para SQL Server con usuario/contraseña:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SynchroDB;User Id=tu_usuario;Password=tu_password;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Para SQL Server Express:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SynchroDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Para LocalDB:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SynchroDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

---

## 📦 PASO 3: Restaurar Paquetes NuGet

Abre una terminal en la carpeta del proyecto y ejecuta:

```bash
dotnet restore
```

Esto instalará todos los paquetes necesarios:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.SignalR
- BCrypt.Net-Next
- Swashbuckle.AspNetCore

---

## 🔨 PASO 4: Compilar el Proyecto

```bash
dotnet build
```

Deberías ver un mensaje de éxito: "Build succeeded."

---

## ▶️ PASO 5: Ejecutar la API

### Opción A: Desde Visual Studio

1. Presiona F5 o haz clic en el botón "▶ ApiSynchro"
2. Se abrirá automáticamente el navegador con Swagger

### Opción B: Desde la terminal

```bash
dotnet run
```

La API estará disponible en:
- HTTPS: https://localhost:7xxx
- HTTP: http://localhost:5xxx

(Los números de puerto pueden variar, revisa la consola)

---

## 📖 PASO 6: Explorar la API con Swagger

1. Una vez ejecutada la API, abre tu navegador
2. Navega a: `https://localhost:7xxx/swagger`
3. Verás la documentación interactiva de todos los endpoints

### Probar un endpoint:

1. Expande el endpoint "POST /api/usuarios/registro"
2. Haz clic en "Try it out"
3. Modifica el JSON de ejemplo
4. Haz clic en "Execute"
5. Verás la respuesta del servidor

---

## 🧪 PASO 7: Probar la API (Primera Prueba)

### Test 1: Registrar un usuario

**Request:**
```http
POST https://localhost:7xxx/api/usuarios/registro
Content-Type: application/json

{
  "nombre": "Juan Pérez",
  "email": "juan@test.com",
  "contrasena": "Password123!",
  "fechaNacimiento": "1995-05-15",
  "ciudad": "Madrid",
  "genero": "Masculino"
}
```

**Respuesta esperada:** Status 201 Created con los datos del usuario

### Test 2: Login

**Request:**
```http
POST https://localhost:7xxx/api/usuarios/login
Content-Type: application/json

{
  "email": "juan@test.com",
  "contrasena": "Password123!"
}
```

**Respuesta esperada:** Status 200 OK con token y datos del usuario

### Test 3: Obtener usuarios

**Request:**
```http
GET https://localhost:7xxx/api/usuarios
```

**Respuesta esperada:** Status 200 OK con array de usuarios

---

## 💬 PASO 8: Probar SignalR (Chat)

### Opción A: Usando el cliente de ejemplo en JavaScript

1. Revisa el archivo `SignalR-Client-Example.js`
2. Copia el código a tu proyecto frontend
3. Instala el paquete: `npm install @microsoft/signalr`
4. Adapta el código a tu framework (React, Vue, Angular, etc.)

### Opción B: Prueba rápida con HTML

Crea un archivo `test-chat.html`:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Test Chat</title>
    <script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@latest/dist/browser/signalr.min.js"></script>
</head>
<body>
    <h1>Chat Test</h1>
    <div id="mensajes"></div>
    <input type="text" id="mensaje" placeholder="Escribe un mensaje..." />
    <button onclick="enviar()">Enviar</button>

    <script>
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7xxx/chatHub")
            .build();

        connection.on("RecibirMensaje", (msg) => {
            document.getElementById("mensajes").innerHTML += 
                `<div>${msg.nombreRemitente}: ${msg.mensajeTexto}</div>`;
        });

        connection.start()
            .then(() => {
                console.log("✅ Conectado!");
                connection.invoke("ConectarUsuario", 1);
            })
            .catch(err => console.error("❌ Error:", err));

        function enviar() {
            const texto = document.getElementById("mensaje").value;
            const mensaje = {
                idMatch: 1,
                idDestinatario: 2,
                mensajeTexto: texto,
                tipoMensaje: "texto"
            };

            connection.invoke("EnviarMensaje", 1, mensaje);
            document.getElementById("mensaje").value = "";
        }
    </script>
</body>
</html>
```

---

## 🔍 PASO 9: Verificar la Base de Datos

Después de probar la API, verifica que los datos se guardaron correctamente:

```sql
-- Ver usuarios registrados
SELECT * FROM Usuario

-- Ver sesiones activas
SELECT * FROM Sesion

-- Ver matches creados
SELECT * FROM [Match]

-- Ver mensajes enviados
SELECT * FROM Mensaje
```

---

## 🛠️ Solución de Problemas Comunes

### Error: "Cannot open database 'SynchroDB'"

**Solución:**
- Verifica que la base de datos existe
- Ejecuta el script SQL nuevamente
- Revisa la cadena de conexión en `appsettings.json`

### Error: "Login failed for user"

**Solución:**
- Verifica tus credenciales de SQL Server
- Si usas autenticación de Windows, asegúrate de usar `Trusted_Connection=True`
- Si usas SQL Auth, verifica el usuario y contraseña

### Error: "The certificate chain was issued by an authority that is not trusted"

**Solución:**
- Agrega `TrustServerCertificate=True` a tu cadena de conexión

### Error en SignalR: "Failed to start connection"

**Solución:**
- Verifica que la API esté ejecutándose
- Revisa la URL del hub: debe ser exactamente `/chatHub`
- Verifica la configuración CORS en `Program.cs`

### Puerto ya en uso

**Solución:**
```bash
# Ver qué proceso usa el puerto
netstat -ano | findstr :7xxx

# Matar el proceso (reemplaza PID)
taskkill /PID xxxxx /F
```

---

## 📊 Monitoreo y Logs

### Ver logs en consola

La API muestra logs en tiempo real. Presta atención a:
- ✅ Mensajes de éxito (verde)
- ⚠️ Advertencias (amarillo)
- ❌ Errores (rojo)

### Habilitar logs detallados de Entity Framework

En `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

---

## 🔐 Seguridad

### Cambiar el secreto de sesión (Producción)

En un entorno de producción, considera usar JWT en lugar de tokens simples:

1. Instala: `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`
2. Configura JWT en `Program.cs`
3. Actualiza `UsuarioService` para generar tokens JWT

### Variables de entorno

Para producción, NO guardes credenciales en `appsettings.json`:

```bash
# Windows
set ConnectionStrings__DefaultConnection="Server=prod;Database=SynchroDB;..."

# Linux/Mac
export ConnectionStrings__DefaultConnection="Server=prod;Database=SynchroDB;..."
```

---

## 📈 Próximos Pasos

Una vez que la API funcione correctamente:

1. ✅ Implementa autenticación JWT
2. ✅ Agrega middleware de autorización
3. ✅ Implementa rate limiting
4. ✅ Agrega logging con Serilog
5. ✅ Implementa caché con Redis
6. ✅ Configura Azure Application Insights
7. ✅ Crea tests unitarios
8. ✅ Dockeriza la aplicación

---

## 📚 Recursos Adicionales

- [Documentación de Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Documentación de SignalR](https://docs.microsoft.com/aspnet/core/signalr)
- [Documentación de ASP.NET Core](https://docs.microsoft.com/aspnet/core)

---

## 🆘 Soporte

Si tienes problemas:

1. Revisa los logs de la consola
2. Verifica la configuración de la base de datos
3. Consulta el archivo `README.md`
4. Revisa los ejemplos en `API-Examples.http`

---

## ✅ Checklist de Verificación

- [ ] SQL Server instalado y ejecutándose
- [ ] Base de datos SynchroDB creada
- [ ] Cadena de conexión configurada correctamente
- [ ] Paquetes NuGet restaurados
- [ ] Proyecto compila sin errores
- [ ] API se ejecuta correctamente
- [ ] Swagger funciona (https://localhost:7xxx/swagger)
- [ ] Puedo registrar un usuario
- [ ] Puedo hacer login
- [ ] SignalR se conecta correctamente
- [ ] Puedo enviar mensajes por SignalR

¡Si todos los checkboxes están marcados, tu API está lista para usar! 🎉
