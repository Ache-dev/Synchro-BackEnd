using Dapper.Contrib.Extensions;
using Model;
using Repository.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Implements
{
    /// <summary>
    /// Implementa la persistencia de sesiones con Dapper.Contrib
    /// </summary>
    public class SesionRepository : ISesionRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SesionRepository"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public SesionRepository(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inserta una sesión en la base de datos
        /// </summary>
        /// <param name="sesion">Entidad a insertar</param>
        public async Task<Sesion> Add(Sesion sesion)
        {
            sesion.IdSesion = (int)await _db.InsertAsync(sesion);
            return sesion;
        }

        /// <summary>
        /// Elimina una sesión por su identificador
        /// </summary>
        /// <param name="id">Id de la sesión</param>
        public async Task Delete(int id)
        {
            var entidad = await _db.GetAsync<Sesion>(id);
            if (entidad != null)
            {
                await _db.DeleteAsync(entidad);
            }
        }

        /// <summary>
        /// Actualiza una sesión existente
        /// </summary>
        /// <param name="sesion">Entidad con los datos actualizados</param>
        public async Task<Sesion> Update(Sesion sesion)
        {
            await _db.UpdateAsync(sesion);
            return sesion;
        }
    }
}