using Dapper;
using Model;
using Query.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Query.Implementations
{
    /// <summary>
    /// Implementa las consultas SQL para respuestas de encuesta
    /// </summary>
    public class RespuestaEncuestaQueries : IRespuestaEncuestaQueries
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RespuestaEncuestaQueries"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public RespuestaEncuestaQueries(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Obtiene la lista completa de respuestas de encuesta
        /// </summary>
        public async Task<IEnumerable<RespuestaEncuesta>> GetAll()
            => await _db.QueryAsync<RespuestaEncuesta>("SELECT * FROM RespuestaEncuesta");

        /// <summary>
        /// Obtiene una respuesta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la respuesta</param>
        public async Task<RespuestaEncuesta?> GetById(int id)
            => await _db.QueryFirstOrDefaultAsync<RespuestaEncuesta>("SELECT * FROM RespuestaEncuesta WHERE IdRespuesta = @id", new { id });
    }
}