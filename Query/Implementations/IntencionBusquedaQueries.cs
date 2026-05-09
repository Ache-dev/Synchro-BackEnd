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
    /// Implementa las consultas SQL para intenciones de búsqueda
    /// </summary>
    public class IntencionBusquedaQueries : IIntencionBusquedaQueries
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="IntencionBusquedaQueries"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public IntencionBusquedaQueries(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Obtiene la lista completa de intenciones de búsqueda
        /// </summary>
        public async Task<IEnumerable<IntencionBusqueda>> GetAll()
            => await _db.QueryAsync<IntencionBusqueda>("SELECT * FROM IntencionBusqueda");

        /// <summary>
        /// Obtiene una intención de búsqueda por su identificador
        /// </summary>
        /// <param name="id">Id de la intención de búsqueda</param>
        public async Task<IntencionBusqueda?> GetById(int id)
            => await _db.QueryFirstOrDefaultAsync<IntencionBusqueda>("SELECT * FROM IntencionBusqueda WHERE IdIntencion = @id", new { id });
    }
}