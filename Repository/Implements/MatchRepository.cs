using Dapper.Contrib.Extensions;
using Model;
using Repository.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Implements
{
    /// <summary>
    /// Implementa la persistencia de matches con Dapper.Contrib
    /// </summary>
    public class MatchRepository : IMatchRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MatchRepository"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public MatchRepository(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inserta un match en la base de datos
        /// </summary>
        /// <param name="match">Entidad a insertar</param>
        public async Task<Match> Add(Match match)
        {
            match.IdMatch = (int)await _db.InsertAsync(match);
            return match;
        }

        /// <summary>
        /// Elimina un match por su identificador
        /// </summary>
        /// <param name="id">Id del match</param>
        public async Task Delete(int id)
        {
            var entidad = await _db.GetAsync<Match>(id);
            if (entidad != null)
            {
                await _db.DeleteAsync(entidad);
            }
        }

        /// <summary>
        /// Actualiza un match existente
        /// </summary>
        /// <param name="match">Entidad con los datos actualizados</param>
        public async Task<Match> Update(Match match)
        {
            await _db.UpdateAsync(match);
            return match;
        }
    }
}