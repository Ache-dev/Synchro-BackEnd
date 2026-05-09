using System.Threading.Tasks;
using Model;

namespace Repository.Interfaces
{
    /// <summary>
    /// Define las operaciones de persistencia para matches
    /// </summary>
    public interface IMatchRepository
    {
        /// <summary>
        /// Inserta un nuevo match
        /// </summary>
        /// <param name="match">Entidad a insertar</param>
        Task<Match> Add(Match match);

        /// <summary>
        /// Actualiza un match existente
        /// </summary>
        /// <param name="match">Entidad con los datos actualizados</param>
        Task<Match> Update(Match match);

        /// <summary>
        /// Elimina un match por su identificador
        /// </summary>
        /// <param name="id">Id del match</param>
        Task Delete(int id);
    }
}