using ApiSynchro.Models;
using Microsoft.AspNetCore.SignalR;

namespace ApiSynchro.Hubs
{
    public sealed class ChatHub : Hub
    {
        private readonly Repository _repository;
        private readonly ILogger<ChatHub> _logger;
        private static readonly Dictionary<int, string> UsuariosConectados = new();

        public ChatHub(Repository repository, ILogger<ChatHub> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task ConectarUsuario(int idUsuario)
        {
            UsuariosConectados[idUsuario] = Context.ConnectionId;
            _logger.LogInformation("Usuario conectado por SignalR: {IdUsuario}", idUsuario);
            return Clients.Caller.SendAsync("UsuarioConectado", true);
        }

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

        public async Task NotificarEscribiendo(int idUsuarioReceptor, bool escribiendo)
        {
            if (UsuariosConectados.TryGetValue(idUsuarioReceptor, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("UsuarioEscribiendo", escribiendo);
            }
        }

        public async Task<IEnumerable<Mensaje>> ObtenerHistorialChat(int idMatch)
        {
            return await _repository.ObtenerMensajesPorMatchAsync(idMatch);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var usuario = UsuariosConectados.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (usuario.Key != 0)
                UsuariosConectados.Remove(usuario.Key);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
