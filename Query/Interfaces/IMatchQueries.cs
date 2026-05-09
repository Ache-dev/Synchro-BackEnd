using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Query.Interfaces
{
    /// <summary>
    /// Define las operaciones de lectura para matches
    /// </summary>
    public interface IMatchQueries
    {
        /// <summary>
        /// Obtiene la lista completa de matches
        /// </summary>
        Task<IEnumerable<Match>> GetAll();

        /// <summary>
        /// Obtiene un match por su identificador
        /// </summary>
        /// <param name="id">Id del match</param>
        Task<Match?> GetById(int id);
    }
}