using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Query.Interfaces
{
    /// <summary>
    /// Define las operaciones de lectura para sesiones
    /// </summary>
    public interface ISesionQueries
    {
        /// <summary>
        /// Obtiene la lista completa de sesiones
        /// </summary>
        Task<IEnumerable<Sesion>> GetAll();

        /// <summary>
        /// Obtiene una sesión por su identificador
        /// </summary>
        /// <param name="id">Id de la sesión</param>
        Task<Sesion?> GetById(int id);
    }
}