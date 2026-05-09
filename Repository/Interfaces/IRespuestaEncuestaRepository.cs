using System.Threading.Tasks;
using Model;

namespace Repository.Interfaces
{
    /// <summary>
    /// Define las operaciones de persistencia para respuestas de encuesta
    /// </summary>
    public interface IRespuestaEncuestaRepository
    {
        /// <summary>
        /// Inserta una nueva respuesta de encuesta
        /// </summary>
        /// <param name="respuestaEncuesta">Entidad a insertar</param>
        Task<RespuestaEncuesta> Add(RespuestaEncuesta respuestaEncuesta);

        /// <summary>
        /// Actualiza una respuesta de encuesta existente
        /// </summary>
        /// <param name="respuestaEncuesta">Entidad con los datos actualizados</param>
        Task<RespuestaEncuesta> Update(RespuestaEncuesta respuestaEncuesta);

        /// <summary>
        /// Elimina una respuesta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la respuesta</param>
        Task Delete(int id);
    }
}