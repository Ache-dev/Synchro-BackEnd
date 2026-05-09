using System.Threading.Tasks;
using Model;

namespace Repository.Interfaces
{
    /// <summary>
    /// Define las operaciones de persistencia para preguntas de encuesta
    /// </summary>
    public interface IPreguntaEncuestaRepository
    {
        /// <summary>
        /// Inserta una nueva pregunta de encuesta
        /// </summary>
        /// <param name="preguntaEncuesta">Entidad a insertar</param>
        Task<PreguntaEncuesta> Add(PreguntaEncuesta preguntaEncuesta);

        /// <summary>
        /// Actualiza una pregunta de encuesta existente
        /// </summary>
        /// <param name="preguntaEncuesta">Entidad con los datos actualizados</param>
        Task<PreguntaEncuesta> Update(PreguntaEncuesta preguntaEncuesta);

        /// <summary>
        /// Elimina una pregunta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la pregunta</param>
        Task Delete(int id);
    }
}