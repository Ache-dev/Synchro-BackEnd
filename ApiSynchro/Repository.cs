using System.Security.Cryptography;
using System.Text;
using ApiSynchro.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ApiSynchro
{
    /// <summary>
    /// Implementación del acceso a datos de la API mediante Dapper y SQL Server.
    /// Centraliza operaciones CRUD y procesos de negocio persistentes sin capa de servicios intermedia.
    /// </summary>
    public sealed class Repository : IRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa una nueva instancia del repositorio usando la cadena de conexión configurada.
        /// </summary>
        /// <param name="configuration">Proveedor de configuración de la aplicación.</param>
        /// <exception cref="InvalidOperationException">Se produce cuando no existe la cadena <c>DefaultConnection</c>.</exception>
        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");
        }

        private SqlConnection CreateConnection() => new(_connectionString);

        private static string HashPassword(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value ?? string.Empty));
            return Convert.ToHexString(bytes);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Usuario>(Query.UsuariosSelectAll);
        }

        /// <inheritdoc />
        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Usuario>(Query.UsuarioSelectById, new { id });
        }

        /// <inheritdoc />
        public async Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Usuario>(Query.UsuarioSelectByEmail, new { email });
        }

        /// <inheritdoc />
        public async Task<int> RegistrarUsuarioAsync(Usuario usuario)
        {
            usuario.Contrasena = HashPassword(usuario.Contrasena);
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.UsuarioInsert, usuario);
        }

        /// <inheritdoc />
        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario, bool actualizarPassword)
        {
            if (actualizarPassword)
                usuario.Contrasena = HashPassword(usuario.Contrasena);

            using var connection = CreateConnection();
            var sql = actualizarPassword ? Query.UsuarioUpdate : Query.UsuarioUpdateWithoutPassword;
            return await connection.ExecuteAsync(sql, usuario) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.UsuarioDelete, new { id }) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> ActualizarEmbeddingUsuarioAsync(int id, string embedding)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.UsuarioUpdateEmbedding, new { id, embedding }) > 0;
        }

        /// <inheritdoc />
        public async Task<Sesion?> ObtenerSesionPorTokenAsync(string token)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Sesion>(Query.SesionSelectByToken, new { token });
        }

        /// <inheritdoc />
        public async Task<int> CrearSesionAsync(Sesion sesion)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.SesionInsert, sesion);
        }

        /// <inheritdoc />
        public async Task<bool> EliminarSesionPorTokenAsync(string token)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.SesionDeleteByToken, new { token }) > 0;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Match>> ObtenerMatchesAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Match>(Query.MatchSelectAll);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Match>> ObtenerMatchesPorUsuarioAsync(int idUsuario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Match>(Query.MatchSelectByUser, new { idUsuario });
        }

        /// <inheritdoc />
        public async Task<Match?> ObtenerMatchPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Match>(Query.MatchSelectById, new { id });
        }

        /// <inheritdoc />
        public async Task<int> CrearMatchAsync(Match match)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.MatchInsert, match);
        }

        /// <inheritdoc />
        public async Task<bool> ActualizarMatchAsync(Match match)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MatchUpdate, match) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> ActualizarEstadoMatchAsync(int id, bool estado)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MatchUpdateEstado, new { id, estado, fechaActualizacion = DateTime.Now }) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> EliminarMatchAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(Query.MensajeDeleteByMatch, new { idMatch = id });
            return await connection.ExecuteAsync(Query.MatchDelete, new { id }) > 0;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Mensaje>> ObtenerMensajesPorMatchAsync(int idMatch)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Mensaje>(Query.MensajeSelectByMatch, new { idMatch });
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Mensaje>> ObtenerConversacionAsync(int idRemitente, int idDestinatario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Mensaje>(Query.MensajeSelectConversation, new { idRemitente, idDestinatario });
        }

        /// <inheritdoc />
        public async Task<Mensaje?> ObtenerMensajePorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Mensaje>(Query.MensajeSelectById, new { id });
        }

        /// <inheritdoc />
        public async Task<int> CrearMensajeAsync(Mensaje mensaje)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.MensajeInsert, mensaje);
        }

        /// <inheritdoc />
        public async Task<bool> MarcarMensajeComoLeidoAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MensajeMarkAsRead, new { id }) > 0;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Mensaje>> ObtenerMensajesNoLeidosAsync(int idUsuario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Mensaje>(Query.MensajeSelectNoLeidos, new { idUsuario });
        }

        /// <inheritdoc />
        public async Task<bool> EliminarMensajeAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MensajeDelete, new { id }) > 0;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PreguntaEncuesta>> ObtenerPreguntasAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<PreguntaEncuesta>(Query.PreguntasSelectAll);
        }

        /// <inheritdoc />
        public async Task<PreguntaEncuesta?> ObtenerPreguntaPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<PreguntaEncuesta>(Query.PreguntaSelectById, new { id });
        }

        /// <inheritdoc />
        public async Task<int> CrearPreguntaAsync(PreguntaEncuesta pregunta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.PreguntaInsert, pregunta);
        }

        /// <inheritdoc />
        public async Task<bool> ActualizarPreguntaAsync(PreguntaEncuesta pregunta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.PreguntaUpdate, pregunta) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> EliminarPreguntaAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.PreguntaDelete, new { id }) > 0;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RespuestaEncuesta>> ObtenerRespuestasAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<RespuestaEncuesta>(Query.RespuestasSelectAll);
        }

        /// <inheritdoc />
        public async Task<RespuestaEncuesta?> ObtenerRespuestaPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<RespuestaEncuesta>(Query.RespuestaSelectById, new { id });
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RespuestaEncuesta>> ObtenerRespuestasPorUsuarioAsync(int idUsuario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<RespuestaEncuesta>(Query.RespuestasSelectByUsuario, new { idUsuario });
        }

        /// <inheritdoc />
        public async Task<int> CrearRespuestaAsync(RespuestaEncuesta respuesta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.RespuestaInsert, respuesta);
        }

        /// <inheritdoc />
        public async Task<bool> ActualizarRespuestaAsync(RespuestaEncuesta respuesta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.RespuestaUpdate, respuesta) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> EliminarRespuestaAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.RespuestaDelete, new { id }) > 0;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IntencionBusqueda>> ObtenerIntencionesAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<IntencionBusqueda>(Query.IntencionesSelectAll);
        }

        /// <inheritdoc />
        public async Task<IntencionBusqueda?> ObtenerIntencionPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<IntencionBusqueda>(Query.IntencionSelectById, new { id });
        }

        /// <inheritdoc />
        public async Task<int> CrearIntencionAsync(IntencionBusqueda intencion)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.IntencionInsert, intencion);
        }

        /// <inheritdoc />
        public async Task<bool> ActualizarIntencionAsync(IntencionBusqueda intencion)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.IntencionUpdate, intencion) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> EliminarIntencionAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.IntencionDelete, new { id }) > 0;
        }

        /// <inheritdoc />
        public string CalcularHash(string valor) => HashPassword(valor);

        /// <inheritdoc />
        public async Task<Match?> SincronizarMatchAsync(int idUsuario)
        {
            using var connection = CreateConnection();
            var targetUser = await connection.QuerySingleOrDefaultAsync<Usuario>(Query.UsuarioSelectById, new { id = idUsuario });
            if (targetUser == null || string.IsNullOrEmpty(targetUser.EmbeddingPerfil))
            {
                return null;
            }

            var others = await connection.QueryAsync<Usuario>(Query.UsuariosSelectForMatching);
            var bestMatch = others.Where(u => u.IdUsuario != idUsuario)
                                  .Select(u => new
                                  {
                                      User = u,
                                      Score = Utils.MathUtils.CosineSimilarity(
                                          Utils.MathUtils.ParseEmbedding(targetUser.EmbeddingPerfil),
                                          Utils.MathUtils.ParseEmbedding(u.EmbeddingPerfil))
                                  })
                                  .Where(x => x.Score > 0.5)
                                  .OrderByDescending(x => x.Score)
                                  .FirstOrDefault();

            if (bestMatch == null)
            {
                return null;
            }

            var match = new Match
            {
                IdUsuario1 = idUsuario,
                IdUsuario2 = bestMatch.User.IdUsuario,
                Compatibilidad = (int)(bestMatch.Score * 100),
                ExplicacionAfinidad = $"Tu frecuencia resuena un {(int)(bestMatch.Score * 100)}% con {bestMatch.User.Nombre}. Comparten patrones de interés profundos detectados por nuestra IA.",
                FechaMatch = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                SugerenciaIA = "¡Es un match científico! Rompe el hielo preguntando sobre sus intereses compartidos.",
                Estado = true
            };

            var id = await CrearMatchAsync(match);
            match.IdMatch = id;
            return match;
        }
    }
}
