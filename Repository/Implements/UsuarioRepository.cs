using Dapper.Contrib.Extensions;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repository.Implements
{
    /// <summary>
    /// Implementa la persistencia de usuarios con Dapper.Contrib
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="UsuarioRepository"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public UsuarioRepository(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inserta un usuario en la base de datos
        /// </summary>
        /// <param name="usuario">Entidad a insertar</param>
        public async Task<Usuario> Add(Usuario usuario)
        {
            try
            {
                usuario.IdUsuario = (int)await _db.InsertAsync(usuario);
                return usuario;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Elimina un usuario por su identificador
        /// </summary>
        /// <param name="id">Id del usuario</param>
        public async Task Delete(int id)
        {
            try
            {
                var u = await _db.GetAsync<Usuario>(id);
                if (u != null)
                {
                    await _db.DeleteAsync(u);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="usuario">Entidad con los datos actualizados</param>
        public async Task<Usuario> Update(Usuario usuario)
        {
            try
            {
                await _db.UpdateAsync(usuario);
                return usuario;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}