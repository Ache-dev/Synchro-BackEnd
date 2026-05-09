using System.Threading.Tasks;
using Model;

namespace Repository.Interfaces
{
    /// <summary>
    /// Define las operaciones de persistencia para mensajes
    /// </summary>
    public interface IMensajeRepository
    {
        /// <summary>
        /// Inserta un nuevo mensaje
        /// </summary>
        /// <param name="mensaje">Entidad a insertar</param>
        Task<Mensaje> Add(Mensaje mensaje);

        /// <summary>
        /// Actualiza un mensaje existente
        /// </summary>
        /// <param name="mensaje">Entidad con los datos actualizados</param>
        Task<Mensaje> Update(Mensaje mensaje);

        /// <summary>
        /// Elimina un mensaje por su identificador
        /// </summary>
        /// <param name="id">Id del mensaje</param>
        Task Delete(int id);
    }
}