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
    /// Implementa las consultas SQL para mensajes
    /// </summary>
    public class MensajeQueries : IMensajeQueries
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MensajeQueries"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public MensajeQueries(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Obtiene la lista completa de mensajes
        /// </summary>
        public async Task<IEnumerable<Mensaje>> GetAll()
            => await _db.QueryAsync<Mensaje>("SELECT * FROM Mensaje");

        /// <summary>
        /// Obtiene un mensaje por su identificador
        /// </summary>
        /// <param name="id">Id del mensaje</param>
        public async Task<Mensaje?> GetById(int id)
            => await _db.QueryFirstOrDefaultAsync<Mensaje>("SELECT * FROM Mensaje WHERE IdMensaje = @id", new { id });
    }
}