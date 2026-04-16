using ApiSynchro.Models;

namespace ApiSynchro
{
    /// <summary>
    /// Define el contrato de acceso a datos para los módulos de usuarios, sesiones, matches,
    /// mensajería, encuestas e intenciones de búsqueda.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Obtiene la lista completa de usuarios registrados.
        /// </summary>
        Task<IEnumerable<Usuario>> ObtenerUsuariosAsync();

        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);

        /// <summary>
        /// Busca un usuario por correo electrónico.
        /// </summary>
        Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email);

        /// <summary>
        /// Registra un nuevo usuario y devuelve su identificador.
        /// </summary>
        Task<int> RegistrarUsuarioAsync(Usuario usuario);

        /// <summary>
        /// Actualiza la información de un usuario existente.
        /// </summary>
        Task<bool> ActualizarUsuarioAsync(Usuario usuario, bool actualizarPassword);

        /// <summary>
        /// Elimina un usuario por su identificador.
        /// </summary>
        Task<bool> EliminarUsuarioAsync(int id);

        /// <summary>
        /// Actualiza el embedding de perfil de un usuario.
        /// </summary>
        Task<bool> ActualizarEmbeddingUsuarioAsync(int id, string embedding);

        /// <summary>
        /// Obtiene una sesión activa por token.
        /// </summary>
        Task<Sesion?> ObtenerSesionPorTokenAsync(string token);

        /// <summary>
        /// Crea una nueva sesión y devuelve su identificador.
        /// </summary>
        Task<int> CrearSesionAsync(Sesion sesion);

        /// <summary>
        /// Elimina una sesión por token.
        /// </summary>
        Task<bool> EliminarSesionPorTokenAsync(string token);

        /// <summary>
        /// Obtiene todos los matches del sistema.
        /// </summary>
        Task<IEnumerable<Match>> ObtenerMatchesAsync();

        /// <summary>
        /// Obtiene los matches asociados a un usuario.
        /// </summary>
        Task<IEnumerable<Match>> ObtenerMatchesPorUsuarioAsync(int idUsuario);

        /// <summary>
        /// Obtiene un match por su identificador.
        /// </summary>
        Task<Match?> ObtenerMatchPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo match y devuelve su identificador.
        /// </summary>
        Task<int> CrearMatchAsync(Match match);

        /// <summary>
        /// Actualiza un match existente.
        /// </summary>
        Task<bool> ActualizarMatchAsync(Match match);

        /// <summary>
        /// Actualiza el estado de un match.
        /// </summary>
        Task<bool> ActualizarEstadoMatchAsync(int id, bool estado);

        /// <summary>
        /// Elimina un match por su identificador.
        /// </summary>
        Task<bool> EliminarMatchAsync(int id);

        /// <summary>
        /// Obtiene los mensajes de un match.
        /// </summary>
        Task<IEnumerable<Mensaje>> ObtenerMensajesPorMatchAsync(int idMatch);

        /// <summary>
        /// Obtiene la conversación entre dos usuarios.
        /// </summary>
        Task<IEnumerable<Mensaje>> ObtenerConversacionAsync(int idRemitente, int idDestinatario);

        /// <summary>
        /// Obtiene un mensaje por su identificador.
        /// </summary>
        Task<Mensaje?> ObtenerMensajePorIdAsync(int id);

        /// <summary>
        /// Crea un mensaje y devuelve su identificador.
        /// </summary>
        Task<int> CrearMensajeAsync(Mensaje mensaje);

        /// <summary>
        /// Marca un mensaje como leído.
        /// </summary>
        Task<bool> MarcarMensajeComoLeidoAsync(int id);

        /// <summary>
        /// Obtiene mensajes no leídos de un usuario.
        /// </summary>
        Task<IEnumerable<Mensaje>> ObtenerMensajesNoLeidosAsync(int idUsuario);

        /// <summary>
        /// Elimina un mensaje por su identificador.
        /// </summary>
        Task<bool> EliminarMensajeAsync(int id);

        /// <summary>
        /// Obtiene todas las preguntas de encuesta.
        /// </summary>
        Task<IEnumerable<PreguntaEncuesta>> ObtenerPreguntasAsync();

        /// <summary>
        /// Obtiene una pregunta por su identificador.
        /// </summary>
        Task<PreguntaEncuesta?> ObtenerPreguntaPorIdAsync(int id);

        /// <summary>
        /// Crea una pregunta y devuelve su identificador.
        /// </summary>
        Task<int> CrearPreguntaAsync(PreguntaEncuesta pregunta);

        /// <summary>
        /// Actualiza una pregunta existente.
        /// </summary>
        Task<bool> ActualizarPreguntaAsync(PreguntaEncuesta pregunta);

        /// <summary>
        /// Elimina una pregunta por su identificador.
        /// </summary>
        Task<bool> EliminarPreguntaAsync(int id);

        /// <summary>
        /// Obtiene todas las respuestas de encuestas.
        /// </summary>
        Task<IEnumerable<RespuestaEncuesta>> ObtenerRespuestasAsync();

        /// <summary>
        /// Obtiene una respuesta por su identificador.
        /// </summary>
        Task<RespuestaEncuesta?> ObtenerRespuestaPorIdAsync(int id);

        /// <summary>
        /// Obtiene las respuestas de encuesta de un usuario.
        /// </summary>
        Task<IEnumerable<RespuestaEncuesta>> ObtenerRespuestasPorUsuarioAsync(int idUsuario);

        /// <summary>
        /// Crea una respuesta y devuelve su identificador.
        /// </summary>
        Task<int> CrearRespuestaAsync(RespuestaEncuesta respuesta);

        /// <summary>
        /// Actualiza una respuesta existente.
        /// </summary>
        Task<bool> ActualizarRespuestaAsync(RespuestaEncuesta respuesta);

        /// <summary>
        /// Elimina una respuesta por su identificador.
        /// </summary>
        Task<bool> EliminarRespuestaAsync(int id);

        /// <summary>
        /// Obtiene todas las intenciones de búsqueda.
        /// </summary>
        Task<IEnumerable<IntencionBusqueda>> ObtenerIntencionesAsync();

        /// <summary>
        /// Obtiene una intención por su identificador.
        /// </summary>
        Task<IntencionBusqueda?> ObtenerIntencionPorIdAsync(int id);

        /// <summary>
        /// Crea una intención y devuelve su identificador.
        /// </summary>
        Task<int> CrearIntencionAsync(IntencionBusqueda intencion);

        /// <summary>
        /// Actualiza una intención existente.
        /// </summary>
        Task<bool> ActualizarIntencionAsync(IntencionBusqueda intencion);

        /// <summary>
        /// Elimina una intención por su identificador.
        /// </summary>
        Task<bool> EliminarIntencionAsync(int id);

        /// <summary>
        /// Calcula el hash seguro de un valor de texto.
        /// </summary>
        string CalcularHash(string valor);

        /// <summary>
        /// Ejecuta el proceso de sincronización para generar un match por afinidad.
        /// </summary>
        Task<Match?> SincronizarMatchAsync(int idUsuario);
    }
}
