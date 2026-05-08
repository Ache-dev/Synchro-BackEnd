using Dapper;
using Query.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Model;

namespace Query.Implementations
{
    /// <summary>
    /// Implementa las consultas SQL de lectura para la entidad Usuario
    /// </summary>
    public class UsuarioQueries : IUsuarioQueries
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="UsuarioQueries"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public UsuarioQueries(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Obtiene la lista completa de usuarios
        /// </summary>
        public async Task<IEnumerable<Usuario>> GetAll()
            => await _db.QueryAsync<Usuario>("SELECT * FROM Usuario");

        /// <summary>
        /// Busca un usuario por su email
        /// </summary>
        /// <param name="email">Email del usuario</param>
        public Task<Usuario?> GetByEmail(string email)
            => _db.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM Usuario WHERE Email = @email", new { email });

        /// <summary>
        /// Obtiene un usuario por su identificador
        /// </summary>
        /// <param name="id">Id del usuario</param>
        public Task<Usuario?> GetById(int id)
            => _db.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM Usuario WHERE IdUsuario = @id", new { id });
    }
}