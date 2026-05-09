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
    /// Implementa las consultas SQL para sesiones
    /// </summary>
    public class SesionQueries : ISesionQueries
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SesionQueries"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public SesionQueries(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Obtiene la lista completa de sesiones
        /// </summary>
        public async Task<IEnumerable<Sesion>> GetAll()
            => await _db.QueryAsync<Sesion>("SELECT * FROM Sesion");

        /// <summary>
        /// Obtiene una sesión por su identificador
        /// </summary>
        /// <param name="id">Id de la sesión</param>
        public async Task<Sesion?> GetById(int id)
            => await _db.QueryFirstOrDefaultAsync<Sesion>("SELECT * FROM Sesion WHERE IdSesion = @id", new { id });
    }
}