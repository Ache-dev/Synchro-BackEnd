using Dapper.Contrib.Extensions;
using Model;
using Repository.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Implements
{
    /// <summary>
    /// Implementa la persistencia de intenciones de búsqueda con Dapper.Contrib
    /// </summary>
    public class IntencionBusquedaRepository : IIntencionBusquedaRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="IntencionBusquedaRepository"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public IntencionBusquedaRepository(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inserta una intención de búsqueda en la base de datos
        /// </summary>
        /// <param name="intencionBusqueda">Entidad a insertar</param>
        public async Task<IntencionBusqueda> Add(IntencionBusqueda intencionBusqueda)
        {
            intencionBusqueda.IdIntencion = (int)await _db.InsertAsync(intencionBusqueda);
            return intencionBusqueda;
        }

        /// <summary>
        /// Elimina una intención de búsqueda por su identificador
        /// </summary>
        /// <param name="id">Id de la intención</param>
        public async Task Delete(int id)
        {
            var entidad = await _db.GetAsync<IntencionBusqueda>(id);
            if (entidad != null)
            {
                await _db.DeleteAsync(entidad);
            }
        }

        /// <summary>
        /// Actualiza una intención de búsqueda existente
        /// </summary>
        /// <param name="intencionBusqueda">Entidad con los datos actualizados</param>
        public async Task<IntencionBusqueda> Update(IntencionBusqueda intencionBusqueda)
        {
            await _db.UpdateAsync(intencionBusqueda);
            return intencionBusqueda;
        }
    }
}