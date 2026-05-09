using Dapper.Contrib.Extensions;
using Model;
using Repository.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Implements
{
    /// <summary>
    /// Implementa la persistencia de preguntas de encuesta con Dapper.Contrib
    /// </summary>
    public class PreguntaEncuestaRepository : IPreguntaEncuestaRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="PreguntaEncuestaRepository"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public PreguntaEncuestaRepository(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inserta una pregunta de encuesta en la base de datos
        /// </summary>
        /// <param name="preguntaEncuesta">Entidad a insertar</param>
        public async Task<PreguntaEncuesta> Add(PreguntaEncuesta preguntaEncuesta)
        {
            preguntaEncuesta.IdPregunta = (int)await _db.InsertAsync(preguntaEncuesta);
            return preguntaEncuesta;
        }

        /// <summary>
        /// Elimina una pregunta de encuesta por su identificador
        /// </summary>
        /// <param name="id">Id de la pregunta</param>
        public async Task Delete(int id)
        {
            var entidad = await _db.GetAsync<PreguntaEncuesta>(id);
            if (entidad != null)
            {
                await _db.DeleteAsync(entidad);
            }
        }

        /// <summary>
        /// Actualiza una pregunta de encuesta existente
        /// </summary>
        /// <param name="preguntaEncuesta">Entidad con los datos actualizados</param>
        public async Task<PreguntaEncuesta> Update(PreguntaEncuesta preguntaEncuesta)
        {
            await _db.UpdateAsync(preguntaEncuesta);
            return preguntaEncuesta;
        }
    }
}