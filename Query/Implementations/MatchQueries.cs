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
    /// Implementa las consultas SQL para matches
    /// </summary>
    public class MatchQueries : IMatchQueries
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MatchQueries"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public MatchQueries(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Obtiene la lista completa de matches
        /// </summary>
        public async Task<IEnumerable<Match>> GetAll()
            => await _db.QueryAsync<Match>("SELECT * FROM [Match]");

        /// <summary>
        /// Obtiene un match por su identificador
        /// </summary>
        /// <param name="id">Id del match</param>
        public async Task<Match?> GetById(int id)
            => await _db.QueryFirstOrDefaultAsync<Match>("SELECT * FROM [Match] WHERE IdMatch = @id", new { id });
    }
}