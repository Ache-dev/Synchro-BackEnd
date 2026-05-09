using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Query.Interfaces
{
    /// <summary>
    /// Define las operaciones de lectura para preguntas de encuesta
    /// </summary>
    public interface IPreguntaEncuestaQueries
    {
        /// <summary>
        /// Obtiene la lista completa de preguntas de encuesta
        /// </summary>
        Task<IEnumerable<PreguntaEncuesta>> GetAll();

        /// <summary>
        /// Obtiene una pregunta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la pregunta</param>
        Task<PreguntaEncuesta?> GetById(int id);
    }
}