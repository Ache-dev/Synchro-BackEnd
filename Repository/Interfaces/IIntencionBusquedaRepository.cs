using System.Threading.Tasks;
using Model;

namespace Repository.Interfaces
{
    /// <summary>
    /// Define las operaciones de persistencia para intenciones de búsqueda
    /// </summary>
    public interface IIntencionBusquedaRepository
    {
        /// <summary>
        /// Inserta una nueva intención de búsqueda
        /// </summary>
        /// <param name="intencionBusqueda">Entidad a insertar</param>
        Task<IntencionBusqueda> Add(IntencionBusqueda intencionBusqueda);

        /// <summary>
        /// Actualiza una intención de búsqueda existente
        /// </summary>
        /// <param name="intencionBusqueda">Entidad con los datos actualizados</param>
        Task<IntencionBusqueda> Update(IntencionBusqueda intencionBusqueda);

        /// <summary>
        /// Elimina una intención de búsqueda por su identificador
        /// </summary>
        /// <param name="id">Id de la intención</param>
        Task Delete(int id);
    }
}