using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Query.Interfaces
{
    /// <summary>
    /// Define las operaciones de lectura para intenciones de búsqueda
    /// </summary>
    public interface IIntencionBusquedaQueries
    {
        /// <summary>
        /// Obtiene la lista completa de intenciones de búsqueda
        /// </summary>
        Task<IEnumerable<IntencionBusqueda>> GetAll();

        /// <summary>
        /// Obtiene una intención de búsqueda por su identificador
        /// </summary>
        /// <param name="id">Id de la intención de búsqueda</param>
        Task<IntencionBusqueda?> GetById(int id);
    }
}