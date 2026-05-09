using System.Threading.Tasks;
using Model;

namespace Repository.Interfaces
{
    /// <summary>
    /// Define las operaciones de persistencia para sesiones
    /// </summary>
    public interface ISesionRepository
    {
        /// <summary>
        /// Inserta una nueva sesión
        /// </summary>
        /// <param name="sesion">Entidad a insertar</param>
        Task<Sesion> Add(Sesion sesion);

        /// <summary>
        /// Actualiza una sesión existente
        /// </summary>
        /// <param name="sesion">Entidad con los datos actualizados</param>
        Task<Sesion> Update(Sesion sesion);

        /// <summary>
        /// Elimina una sesión por su identificador
        /// </summary>
        /// <param name="id">Id de la sesión</param>
        Task Delete(int id);
    }
}