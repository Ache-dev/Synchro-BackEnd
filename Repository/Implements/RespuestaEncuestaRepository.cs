using Dapper.Contrib.Extensions;
using Model;
using Repository.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Implements
{
    /// <summary>
    /// Implementa la persistencia de respuestas de encuesta con Dapper.Contrib
    /// </summary>
    public class RespuestaEncuestaRepository : IRespuestaEncuestaRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RespuestaEncuestaRepository"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public RespuestaEncuestaRepository(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inserta una respuesta de encuesta en la base de datos
        /// </summary>
        /// <param name="respuestaEncuesta">Entidad a insertar</param>
        public async Task<RespuestaEncuesta> Add(RespuestaEncuesta respuestaEncuesta)
        {
            respuestaEncuesta.IdRespuesta = (int)await _db.InsertAsync(respuestaEncuesta);
            return respuestaEncuesta;
        }

        /// <summary>
        /// Elimina una respuesta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la respuesta</param>
        public async Task Delete(int id)
        {
            var entidad = await _db.GetAsync<RespuestaEncuesta>(id);
            if (entidad != null)
            {
                await _db.DeleteAsync(entidad);
            }
        }

        /// <summary>
        /// Actualiza una respuesta de encuesta existente
        /// </summary>
        /// <param name="respuestaEncuesta">Entidad con los datos actualizados</param>
        public async Task<RespuestaEncuesta> Update(RespuestaEncuesta respuestaEncuesta)
        {
            await _db.UpdateAsync(respuestaEncuesta);
            return respuestaEncuesta;
        }
    }
}