
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repository.Interfaces
{
    /// <summary>
    /// Define las operaciones de persistencia para usuarios
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Inserta un nuevo usuario
        /// </summary>
        /// <param name="usuario">Entidad a insertar</param>
        Task<Usuario> Add(Usuario usuario);

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="usuario">Entidad con los datos actualizados</param>
        Task<Usuario> Update(Usuario usuario);

        /// <summary>
        /// Elimina un usuario por su identificador
        /// </summary>
        /// <param name="id">Id del usuario</param>
        Task Delete(int id);
    }
}