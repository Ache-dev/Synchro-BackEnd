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
    /// Implementa las consultas SQL para preguntas de encuesta
    /// </summary>
    public class PreguntaEncuestaQueries : IPreguntaEncuestaQueries
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="PreguntaEncuestaQueries"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public PreguntaEncuestaQueries(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Obtiene la lista completa de preguntas de encuesta
        /// </summary>
        public async Task<IEnumerable<PreguntaEncuesta>> GetAll()
            => await _db.QueryAsync<PreguntaEncuesta>("SELECT * FROM PreguntaEncuesta");

        /// <summary>
        /// Obtiene una pregunta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la pregunta</param>
        public async Task<PreguntaEncuesta?> GetById(int id)
            => await _db.QueryFirstOrDefaultAsync<PreguntaEncuesta>("SELECT * FROM PreguntaEncuesta WHERE IdPregunta = @id", new { id });
    }
}