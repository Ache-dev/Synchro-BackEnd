// ===========================================
// EJEMPLO DE CLIENTE SIGNALR PARA CHAT
// ===========================================

// 1. Instalar el paquete de SignalR en tu proyecto frontend:
// npm install @microsoft/signalr

import * as signalR from "@microsoft/signalr";

class ChatService {
    constructor() {
        this.connection = null;
        this.isConnected = false;
    }

    // Iniciar conexión
    async iniciarConexion(hubUrl = "https://localhost:7xxx/chatHub") {
        try {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl(hubUrl, {
                    skipNegotiation: false,
                    transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
                })
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Information)
                .build();

            // Configurar manejadores de eventos
            this.configurarEventos();

            // Iniciar conexión
            await this.connection.start();
            this.isConnected = true;
            console.log("✅ Conectado a SignalR Hub");

            return true;
        } catch (error) {
            console.error("❌ Error al conectar:", error);
            this.isConnected = false;
            return false;
        }
    }

    // Configurar eventos del servidor
    configurarEventos() {
        // Confirmación de conexión
        this.connection.on("UsuarioConectado", (mensaje) => {
            console.log("👤 Usuario conectado:", mensaje);
        });

        // Recibir mensaje nuevo
        this.connection.on("RecibirMensaje", (mensaje) => {
            console.log("📩 Nuevo mensaje recibido:", mensaje);
            this.onMensajeRecibido(mensaje);
        });

        // Confirmación de mensaje enviado
        this.connection.on("MensajeEnviado", (mensaje) => {
            console.log("✉️ Mensaje enviado:", mensaje);
            this.onMensajeEnviado(mensaje);
        });

        // Mensaje leído
        this.connection.on("MensajeLeido", (idMensaje) => {
            console.log("✔️ Mensaje leído:", idMensaje);
            this.onMensajeLeido(idMensaje);
        });

        // Usuario escribiendo
        this.connection.on("UsuarioEscribiendo", (idMatch, escribiendo) => {
            console.log(`✍️ Usuario ${escribiendo ? 'está' : 'dejó de'} escribiendo en match ${idMatch}`);
            this.onUsuarioEscribiendo(idMatch, escribiendo);
        });

        // Manejo de reconexión
        this.connection.onreconnecting((error) => {
            console.warn("🔄 Reconectando...", error);
            this.isConnected = false;
        });

        this.connection.onreconnected((connectionId) => {
            console.log("✅ Reconectado con ID:", connectionId);
            this.isConnected = true;
        });

        this.connection.onclose((error) => {
            console.error("❌ Conexión cerrada:", error);
            this.isConnected = false;
        });
    }

    // Conectar usuario al hub
    async conectarUsuario(idUsuario) {
        if (!this.isConnected) {
            console.error("No hay conexión activa");
            return false;
        }

        try {
            await this.connection.invoke("ConectarUsuario", idUsuario);
            console.log(`✅ Usuario ${idUsuario} conectado al hub`);
            return true;
        } catch (error) {
            console.error("Error al conectar usuario:", error);
            return false;
        }
    }

    // Enviar mensaje
    async enviarMensaje(idRemitente, idMatch, idDestinatario, mensajeTexto, tipoMensaje = "texto") {
        if (!this.isConnected) {
            console.error("No hay conexión activa");
            return null;
        }

        try {
            const mensajeDto = {
                idMatch: idMatch,
                idDestinatario: idDestinatario,
                mensajeTexto: mensajeTexto,
                tipoMensaje: tipoMensaje
            };

            await this.connection.invoke("EnviarMensaje", idRemitente, mensajeDto);
            return true;
        } catch (error) {
            console.error("Error al enviar mensaje:", error);
            return false;
        }
    }

    // Marcar mensaje como leído
    async marcarComoLeido(idMensaje) {
        if (!this.isConnected) {
            console.error("No hay conexión activa");
            return false;
        }

        try {
            await this.connection.invoke("MarcarComoLeido", idMensaje);
            return true;
        } catch (error) {
            console.error("Error al marcar como leído:", error);
            return false;
        }
    }

    // Notificar que está escribiendo
    async notificarEscribiendo(idUsuarioReceptor, idMatch, escribiendo) {
        if (!this.isConnected) {
            console.error("No hay conexión activa");
            return false;
        }

        try {
            await this.connection.invoke("NotificarEscribiendo", idUsuarioReceptor, idMatch, escribiendo);
            return true;
        } catch (error) {
            console.error("Error al notificar escribiendo:", error);
            return false;
        }
    }

    // Obtener historial de chat
    async obtenerHistorialChat(idMatch) {
        if (!this.isConnected) {
            console.error("No hay conexión activa");
            return [];
        }

        try {
            const mensajes = await this.connection.invoke("ObtenerHistorialChat", idMatch);
            return mensajes;
        } catch (error) {
            console.error("Error al obtener historial:", error);
            return [];
        }
    }

    // Obtener mensajes no leídos
    async obtenerMensajesNoLeidos(idUsuario) {
        if (!this.isConnected) {
            console.error("No hay conexión activa");
            return [];
        }

        try {
            const mensajes = await this.connection.invoke("ObtenerMensajesNoLeidos", idUsuario);
            return mensajes;
        } catch (error) {
            console.error("Error al obtener mensajes no leídos:", error);
            return [];
        }
    }

    // Desconectar
    async desconectar() {
        if (this.connection) {
            await this.connection.stop();
            this.isConnected = false;
            console.log("🔌 Desconectado del hub");
        }
    }

    // ============================================
    // CALLBACKS - Override estos métodos en tu UI
    // ============================================

    onMensajeRecibido(mensaje) {
        // Implementar en tu UI
        console.log("Callback: onMensajeRecibido", mensaje);
    }

    onMensajeEnviado(mensaje) {
        // Implementar en tu UI
        console.log("Callback: onMensajeEnviado", mensaje);
    }

    onMensajeLeido(idMensaje) {
        // Implementar en tu UI
        console.log("Callback: onMensajeLeido", idMensaje);
    }

    onUsuarioEscribiendo(idMatch, escribiendo) {
        // Implementar en tu UI
        console.log("Callback: onUsuarioEscribiendo", idMatch, escribiendo);
    }
}

// ===========================================
// EJEMPLO DE USO
// ===========================================

async function ejemploDeUso() {
    // 1. Crear instancia del servicio
    const chatService = new ChatService();

    // 2. Configurar callbacks personalizados
    chatService.onMensajeRecibido = (mensaje) => {
        console.log("📩 NUEVO MENSAJE:", mensaje);
        // Actualizar tu UI aquí
        // Ejemplo: agregarMensajeAlChat(mensaje);
    };

    chatService.onUsuarioEscribiendo = (idMatch, escribiendo) => {
        console.log(`Usuario ${escribiendo ? 'escribiendo...' : 'dejó de escribir'}`);
        // Mostrar indicador en tu UI
        // Ejemplo: mostrarIndicadorEscribiendo(idMatch, escribiendo);
    };

    // 3. Conectar al hub
    const conectado = await chatService.iniciarConexion("https://localhost:7xxx/chatHub");

    if (conectado) {
        // 4. Registrar usuario
        const idUsuario = 1;
        await chatService.conectarUsuario(idUsuario);

        // 5. Obtener mensajes no leídos
        const mensajesNoLeidos = await chatService.obtenerMensajesNoLeidos(idUsuario);
        console.log("Mensajes no leídos:", mensajesNoLeidos);

        // 6. Obtener historial de un match
        const idMatch = 1;
        const historial = await chatService.obtenerHistorialChat(idMatch);
        console.log("Historial del chat:", historial);

        // 7. Enviar mensaje
        await chatService.enviarMensaje(
            1,  // idRemitente
            1,  // idMatch
            2,  // idDestinatario
            "Hola! ¿Cómo estás?",
            "texto"
        );

        // 8. Notificar que está escribiendo
        await chatService.notificarEscribiendo(2, 1, true);

        // Esperar unos segundos...
        setTimeout(async () => {
            await chatService.notificarEscribiendo(2, 1, false);
        }, 3000);

        // 9. Marcar mensaje como leído
        // await chatService.marcarComoLeido(123);
    }
}

// ===========================================
// EJEMPLO REACT
// ===========================================

/*
import React, { useEffect, useState } from 'react';
import { ChatService } from './ChatService';

function ChatComponent() {
    const [chatService] = useState(new ChatService());
    const [mensajes, setMensajes] = useState([]);
    const [escribiendo, setEscribiendo] = useState(false);
    const [textoMensaje, setTextoMensaje] = useState('');

    const idUsuario = 1; // Usuario actual
    const idMatch = 1;   // Match actual
    const idDestinatario = 2; // Usuario destinatario

    useEffect(() => {
        // Configurar callbacks
        chatService.onMensajeRecibido = (mensaje) => {
            setMensajes(prev => [...prev, mensaje]);
            // Reproducir sonido o notificación
        };

        chatService.onUsuarioEscribiendo = (match, escribiendo) => {
            if (match === idMatch) {
                setEscribiendo(escribiendo);
            }
        };

        // Iniciar conexión
        const iniciar = async () => {
            await chatService.iniciarConexion('https://localhost:7xxx/chatHub');
            await chatService.conectarUsuario(idUsuario);

            // Cargar historial
            const historial = await chatService.obtenerHistorialChat(idMatch);
            setMensajes(historial);
        };

        iniciar();

        // Cleanup
        return () => {
            chatService.desconectar();
        };
    }, []);

    const enviarMensaje = async () => {
        if (textoMensaje.trim()) {
            await chatService.enviarMensaje(
                idUsuario,
                idMatch,
                idDestinatario,
                textoMensaje,
                'texto'
            );
            setTextoMensaje('');
        }
    };

    const handleInputChange = (e) => {
        setTextoMensaje(e.target.value);
        // Notificar que está escribiendo
        chatService.notificarEscribiendo(idDestinatario, idMatch, true);

        // Después de 2 segundos sin escribir, notificar que dejó de escribir
        clearTimeout(window.typingTimeout);
        window.typingTimeout = setTimeout(() => {
            chatService.notificarEscribiendo(idDestinatario, idMatch, false);
        }, 2000);
    };

    return (
        <div className="chat-container">
            <div className="mensajes">
                {mensajes.map(msg => (
                    <div key={msg.idMensaje} className={msg.idRemitente === idUsuario ? 'mensaje-enviado' : 'mensaje-recibido'}>
                        <p>{msg.mensajeTexto}</p>
                        <span>{new Date(msg.fechaMensaje).toLocaleTimeString()}</span>
                    </div>
                ))}
                {escribiendo && <div className="escribiendo">Usuario está escribiendo...</div>}
            </div>

            <div className="input-container">
                <input
                    type="text"
                    value={textoMensaje}
                    onChange={handleInputChange}
                    placeholder="Escribe un mensaje..."
                />
                <button onClick={enviarMensaje}>Enviar</button>
            </div>
        </div>
    );
}

export default ChatComponent;
*/

// ===========================================
// EJEMPLO VANILLA JAVASCRIPT / HTML
// ===========================================

/*
<!DOCTYPE html>
<html>
<head>
    <title>Chat Synchro</title>
    <script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@latest/dist/browser/signalr.min.js"></script>
</head>
<body>
    <div id="chat">
        <div id="mensajes"></div>
        <input type="text" id="mensaje" placeholder="Escribe un mensaje..." />
        <button onclick="enviarMensaje()">Enviar</button>
    </div>

    <script>
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7xxx/chatHub")
            .build();

        connection.on("RecibirMensaje", (mensaje) => {
            const div = document.createElement("div");
            div.textContent = `${mensaje.nombreRemitente}: ${mensaje.mensajeTexto}`;
            document.getElementById("mensajes").appendChild(div);
        });

        connection.start()
            .then(() => {
                console.log("Conectado!");
                connection.invoke("ConectarUsuario", 1);
            })
            .catch(err => console.error(err));

        function enviarMensaje() {
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
*/

export default ChatService;
