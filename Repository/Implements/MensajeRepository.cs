using Dapper;
using Model;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Implements
{
    /// <summary>
    /// Implementa la persistencia de mensajes con SQL directo
    /// </summary>
    public class MensajeRepository : IMensajeRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MensajeRepository"/>
        /// </summary>
        /// <param name="db">Conexión a base de datos</param>
        public MensajeRepository(IDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inserta un mensaje en la base de datos
        /// </summary>
        /// <param name="mensaje">Entidad a insertar</param>
        public async Task<Mensaje> Add(Mensaje mensaje)
        {
            const string sql = @"
INSERT INTO Mensaje (IdMatch, IdRemitente, IdDestinatario, Mensaje, FechaMensaje, TipoMensaje, EstadoLeido)
OUTPUT INSERTED.IdMensaje
VALUES (@IdMatch, @IdRemitente, @IdDestinatario, @TextoMensaje, @FechaMensaje, @TipoMensaje, @EstadoLeido)";

            mensaje.IdMensaje = await _db.ExecuteScalarAsync<int>(sql, mensaje);
            return mensaje;
        }

        /// <summary>
        /// Elimina un mensaje por su identificador
        /// </summary>
        /// <param name="id">Id del mensaje</param>
        public async Task Delete(int id)
        {
            await _db.ExecuteAsync("DELETE FROM Mensaje WHERE IdMensaje = @id", new { id });
        }

        /// <summary>
        /// Actualiza un mensaje existente
        /// </summary>
        /// <param name="mensaje">Entidad con los datos actualizados</param>
        public async Task<Mensaje> Update(Mensaje mensaje)
        {
            const string sql = @"
UPDATE Mensaje
SET IdMatch = @IdMatch,
    IdRemitente = @IdRemitente,
    IdDestinatario = @IdDestinatario,
    Mensaje = @TextoMensaje,
    FechaMensaje = @FechaMensaje,
    TipoMensaje = @TipoMensaje,
    EstadoLeido = @EstadoLeido
WHERE IdMensaje = @IdMensaje";

            await _db.ExecuteAsync(sql, mensaje);
            return mensaje;
        }
    }
}