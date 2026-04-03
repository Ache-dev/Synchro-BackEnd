using System.Security.Cryptography;
using System.Text;
using ApiSynchro.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ApiSynchro
{
    public sealed class Repository
    {
        private readonly string _connectionString;

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

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Usuario>(Query.UsuariosSelectAll);
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Usuario>(Query.UsuarioSelectById, new { id });
        }

        public async Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Usuario>(Query.UsuarioSelectByEmail, new { email });
        }

        public async Task<int> RegistrarUsuarioAsync(Usuario usuario)
        {
            usuario.Contrasena = HashPassword(usuario.Contrasena);
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.UsuarioInsert, usuario);
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario, bool actualizarPassword)
        {
            if (actualizarPassword)
                usuario.Contrasena = HashPassword(usuario.Contrasena);

            using var connection = CreateConnection();
            var sql = actualizarPassword ? Query.UsuarioUpdate : Query.UsuarioUpdateWithoutPassword;
            return await connection.ExecuteAsync(sql, usuario) > 0;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.UsuarioDelete, new { id }) > 0;
        }

        public async Task<Sesion?> ObtenerSesionPorTokenAsync(string token)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Sesion>(Query.SesionSelectByToken, new { token });
        }

        public async Task<int> CrearSesionAsync(Sesion sesion)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.SesionInsert, sesion);
        }

        public async Task<bool> EliminarSesionPorTokenAsync(string token)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.SesionDeleteByToken, new { token }) > 0;
        }

        public async Task<IEnumerable<Match>> ObtenerMatchesAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Match>(Query.MatchSelectAll);
        }

        public async Task<IEnumerable<Match>> ObtenerMatchesPorUsuarioAsync(int idUsuario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Match>(Query.MatchSelectByUser, new { idUsuario });
        }

        public async Task<Match?> ObtenerMatchPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Match>(Query.MatchSelectById, new { id });
        }

        public async Task<int> CrearMatchAsync(Match match)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.MatchInsert, match);
        }

        public async Task<bool> ActualizarMatchAsync(Match match)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MatchUpdate, match) > 0;
        }

        public async Task<bool> ActualizarEstadoMatchAsync(int id, bool estado)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MatchUpdateEstado, new { id, estado, fechaActualizacion = DateTime.Now }) > 0;
        }

        public async Task<bool> EliminarMatchAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MatchDelete, new { id }) > 0;
        }

        public async Task<IEnumerable<Mensaje>> ObtenerMensajesPorMatchAsync(int idMatch)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Mensaje>(Query.MensajeSelectByMatch, new { idMatch });
        }

        public async Task<IEnumerable<Mensaje>> ObtenerConversacionAsync(int idRemitente, int idDestinatario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Mensaje>(Query.MensajeSelectConversation, new { idRemitente, idDestinatario });
        }

        public async Task<Mensaje?> ObtenerMensajePorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Mensaje>(Query.MensajeSelectById, new { id });
        }

        public async Task<int> CrearMensajeAsync(Mensaje mensaje)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.MensajeInsert, mensaje);
        }

        public async Task<bool> MarcarMensajeComoLeidoAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MensajeMarkAsRead, new { id }) > 0;
        }

        public async Task<IEnumerable<Mensaje>> ObtenerMensajesNoLeidosAsync(int idUsuario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Mensaje>(Query.MensajeSelectNoLeidos, new { idUsuario });
        }

        public async Task<bool> EliminarMensajeAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.MensajeDelete, new { id }) > 0;
        }

        public async Task<IEnumerable<PreguntaEncuesta>> ObtenerPreguntasAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<PreguntaEncuesta>(Query.PreguntasSelectAll);
        }

        public async Task<PreguntaEncuesta?> ObtenerPreguntaPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<PreguntaEncuesta>(Query.PreguntaSelectById, new { id });
        }

        public async Task<int> CrearPreguntaAsync(PreguntaEncuesta pregunta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.PreguntaInsert, pregunta);
        }

        public async Task<bool> ActualizarPreguntaAsync(PreguntaEncuesta pregunta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.PreguntaUpdate, pregunta) > 0;
        }

        public async Task<bool> EliminarPreguntaAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.PreguntaDelete, new { id }) > 0;
        }

        public async Task<IEnumerable<RespuestaEncuesta>> ObtenerRespuestasAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<RespuestaEncuesta>(Query.RespuestasSelectAll);
        }

        public async Task<RespuestaEncuesta?> ObtenerRespuestaPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<RespuestaEncuesta>(Query.RespuestaSelectById, new { id });
        }

        public async Task<IEnumerable<RespuestaEncuesta>> ObtenerRespuestasPorUsuarioAsync(int idUsuario)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<RespuestaEncuesta>(Query.RespuestasSelectByUsuario, new { idUsuario });
        }

        public async Task<int> CrearRespuestaAsync(RespuestaEncuesta respuesta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.RespuestaInsert, respuesta);
        }

        public async Task<bool> ActualizarRespuestaAsync(RespuestaEncuesta respuesta)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.RespuestaUpdate, respuesta) > 0;
        }

        public async Task<bool> EliminarRespuestaAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.RespuestaDelete, new { id }) > 0;
        }

        public async Task<IEnumerable<IntencionBusqueda>> ObtenerIntencionesAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<IntencionBusqueda>(Query.IntencionesSelectAll);
        }

        public async Task<IntencionBusqueda?> ObtenerIntencionPorIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<IntencionBusqueda>(Query.IntencionSelectById, new { id });
        }

        public async Task<int> CrearIntencionAsync(IntencionBusqueda intencion)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(Query.IntencionInsert, intencion);
        }

        public async Task<bool> ActualizarIntencionAsync(IntencionBusqueda intencion)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.IntencionUpdate, intencion) > 0;
        }

        public async Task<bool> EliminarIntencionAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(Query.IntencionDelete, new { id }) > 0;
        }

        public static string CalcularHash(string valor) => HashPassword(valor);
    }
}
