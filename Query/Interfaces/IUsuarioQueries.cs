using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Query.Interfaces
{
    /// <summary>
    /// Interfaz para las operaciones de lectura de la entidad Usuario
    /// </summary>
    public interface IUsuarioQueries
    {
        /// <summary>
        /// Obtiene la lista completa de usuarios
        /// </summary>
        Task<IEnumerable<Usuario>> GetAll();

        /// <summary>
        /// Obtiene un usuario por su identificador único
        /// </summary>
        /// <param name="id">Id del usuario</param>
        Task<Usuario?> GetById(int id);

        /// <summary>
        /// Busca un usuario por su dirección de correo electrónico
        /// </summary>
        /// <param name="email">Email del usuario</param>
        Task<Usuario?> GetByEmail(string email);
    }
}