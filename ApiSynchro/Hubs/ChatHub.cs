using ApiSynchro.DTOs;
using ApiSynchro.Services;
using Microsoft.AspNetCore.SignalR;

namespace ApiSynchro.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMensajeService _mensajeService;
        private static readonly Dictionary<int, string> _usuariosConectados = new();

        public ChatHub(IMensajeService mensajeService)
        {
            _mensajeService = mensajeService;
        }

        public async Task ConectarUsuario(int idUsuario)
        {
            _usuariosConectados[idUsuario] = Context.ConnectionId;
            await Clients.Caller.SendAsync("UsuarioConectado", "Conectado exitosamente");
        }

        public async Task EnviarMensaje(int idRemitente, MensajeCreateDto mensajeDto)
        {
            var mensaje = await _mensajeService.EnviarMensajeAsync(idRemitente, mensajeDto);

            if (mensaje != null)
            {
                if (_usuariosConectados.TryGetValue(mensajeDto.IdDestinatario, out var connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("RecibirMensaje", mensaje);
                }

                await Clients.Caller.SendAsync("MensajeEnviado", mensaje);
            }
        }

        public async Task MarcarComoLeido(int idMensaje)
        {
            var resultado = await _mensajeService.MarcarComoLeidoAsync(idMensaje);
            if (resultado)
            {
                await Clients.Caller.SendAsync("MensajeLeido", idMensaje);
            }
        }

        public async Task NotificarEscribiendo(int idUsuarioReceptor, int idMatch, bool escribiendo)
        {
            if (_usuariosConectados.TryGetValue(idUsuarioReceptor, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("UsuarioEscribiendo", idMatch, escribiendo);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var usuario = _usuariosConectados.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (usuario.Key != 0)
            {
                _usuariosConectados.Remove(usuario.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<MensajeDto>> ObtenerHistorialChat(int idMatch)
        {
            return await _mensajeService.ObtenerMensajesPorMatchAsync(idMatch);
        }

        public async Task<List<MensajeDto>> ObtenerMensajesNoLeidos(int idUsuario)
        {
            return await _mensajeService.ObtenerMensajesNoLeidosAsync(idUsuario);
        }
    }
}
