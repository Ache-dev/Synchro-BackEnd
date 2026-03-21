using Dapper;
using Synchro.Domain.Entities;
using Synchro.Infrastructure.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Synchro.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDapperConnectionFactory _connectionFactory;
        public UsuarioRepository(IDapperConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT * FROM Usuario WHERE IdUsuario = @Id AND Estado = 1";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT * FROM Usuario WHERE Estado = 1";
            return await connection.QueryAsync<Usuario>(sql);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT * FROM Usuario WHERE Email = @Email AND Estado = 1";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public async Task<int> InsertAsync(Usuario usuario)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO Usuario (Nombre, Email, Contrasena, FechaNacimiento, Ciudad, IntencionBusqueda, Genero, FotoPerfil, IdiomaPreferido, TemaPreferido, BioAI, Estado)
                        VALUES (@Nombre, @Email, @Contrasena, @FechaNacimiento, @Ciudad, @IntencionBusqueda, @Genero, @FotoPerfil, @IdiomaPreferido, @TemaPreferido, @BioAI, @Estado);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.QuerySingleAsync<int>(sql, usuario);
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"UPDATE Usuario SET Nombre = @Nombre, Ciudad = @Ciudad, IntencionBusqueda = @IntencionBusqueda, Genero = @Genero, FotoPerfil = @FotoPerfil, IdiomaPreferido = @IdiomaPreferido, TemaPreferido = @TemaPreferido, BioAI = @BioAI WHERE IdUsuario = @IdUsuario";
            var affected = await connection.ExecuteAsync(sql, usuario);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "UPDATE Usuario SET Estado = 0 WHERE IdUsuario = @Id";
            var affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }
    }
}
