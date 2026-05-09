using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Query.Interfaces
{
    /// <summary>
    /// Define las operaciones de lectura para mensajes
    /// </summary>
    public interface IMensajeQueries
    {
        /// <summary>
        /// Obtiene la lista completa de mensajes
        /// </summary>
        Task<IEnumerable<Mensaje>> GetAll();

        /// <summary>
        /// Obtiene un mensaje por su identificador
        /// </summary>
        /// <param name="id">Id del mensaje</param>
        Task<Mensaje?> GetById(int id);
    }
}