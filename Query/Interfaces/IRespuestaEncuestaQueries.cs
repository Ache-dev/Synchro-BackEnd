using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Query.Interfaces
{
    /// <summary>
    /// Define las operaciones de lectura para respuestas de encuesta
    /// </summary>
    public interface IRespuestaEncuestaQueries
    {
        /// <summary>
        /// Obtiene la lista completa de respuestas de encuesta
        /// </summary>
        Task<IEnumerable<RespuestaEncuesta>> GetAll();

        /// <summary>
        /// Obtiene una respuesta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la respuesta</param>
        Task<RespuestaEncuesta?> GetById(int id);
    }
}