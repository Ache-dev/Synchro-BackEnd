using ApiSynchro.Models;
using Microsoft.AspNetCore.SignalR;

namespace ApiSynchro.Hubs
{
    /// <summary>
    /// Hub de SignalR para mensajería en tiempo real, estado de conexión y eventos de escritura.
    /// </summary>
    public sealed class ChatHub : Hub
    {
        private readonly IRepository _repository;
        private readonly ILogger<ChatHub> _logger;
        private static readonly Dictionary<int, string> UsuariosConectados = new();

        /// <summary>
        /// Inicializa una nueva instancia del hub de chat.
        /// </summary>
        public ChatHub(IRepository repository, ILogger<ChatHub> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Registra al usuario actual como conectado al canal de tiempo real.
        /// </summary>
        public Task ConectarUsuario(int idUsuario)
        {
            UsuariosConectados[idUsuario] = Context.ConnectionId;
            _logger.LogInformation("Usuario conectado por SignalR: {IdUsuario}", idUsuario);
            return Clients.Caller.SendAsync("UsuarioConectado", true);
        }

        /// <summary>
        /// Persiste y envía un mensaje al destinatario conectado y al remitente.
        /// </summary>
        public async Task EnviarMensaje(Mensaje mensaje)
        {
            var id = await _repository.CrearMensajeAsync(mensaje);
            mensaje.IdMensaje = id;

            if (UsuariosConectados.TryGetValue(mensaje.IdDestinatario, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("RecibirMensaje", mensaje);
            }

            await Clients.Caller.SendAsync("MensajeEnviado", mensaje);
        }

        /// <summary>
        /// Notifica al receptor si el usuario está escribiendo o dejó de hacerlo.
        /// </summary>
        public async Task NotificarEscribiendo(int idUsuarioReceptor, bool escribiendo)
        {
            if (UsuariosConectados.TryGetValue(idUsuarioReceptor, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("UsuarioEscribiendo", escribiendo);
            }
        }

        /// <summary>
        /// Obtiene el historial de mensajes asociado a un match.
        /// </summary>
        public async Task<IEnumerable<Mensaje>> ObtenerHistorialChat(int idMatch)
        {
            return await _repository.ObtenerMensajesPorMatchAsync(idMatch);
        }

        /// <summary>
        /// Limpia el registro interno de conexión cuando un cliente se desconecta.
        /// </summary>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var usuario = UsuariosConectados.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (usuario.Key != 0)
            {
                UsuariosConectados.Remove(usuario.Key);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
