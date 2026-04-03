namespace ApiSynchro
{
    public static class Query
    {
        public const string UsuariosSelectAll = @"
SELECT IdUsuario, Nombre, Email, FechaNacimiento, Ciudad, IntencionBusqueda, Genero, FotoPerfil, IdiomaPreferido, TemaPreferido, BioAI, EmbeddingPerfil, Estado
FROM [Usuario]
ORDER BY IdUsuario DESC;";

        public const string UsuarioSelectById = @"
SELECT IdUsuario, Nombre, Email, FechaNacimiento, Ciudad, IntencionBusqueda, Genero, FotoPerfil, IdiomaPreferido, TemaPreferido, BioAI, EmbeddingPerfil, Estado
FROM [Usuario]
WHERE IdUsuario = @id;";

        public const string UsuarioSelectByEmail = @"
SELECT IdUsuario, Nombre, Email, Contrasena, FechaNacimiento, Ciudad, IntencionBusqueda, Genero, FotoPerfil, IdiomaPreferido, TemaPreferido, BioAI, EmbeddingPerfil, Estado
FROM [Usuario]
WHERE Email = @email;";

        public const string UsuarioInsert = @"
INSERT INTO [Usuario]
(Nombre, Email, Contrasena, FechaNacimiento, Ciudad, IntencionBusqueda, Genero, FotoPerfil, IdiomaPreferido, TemaPreferido, BioAI, EmbeddingPerfil, Estado)
VALUES
(@Nombre, @Email, @Contrasena, @FechaNacimiento, @Ciudad, @IntencionBusqueda, @Genero, @FotoPerfil, @IdiomaPreferido, @TemaPreferido, @BioAI, @EmbeddingPerfil, @Estado);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string UsuarioUpdate = @"
UPDATE [Usuario]
SET Nombre = @Nombre,
    Email = @Email,
    Contrasena = @Contrasena,
    FechaNacimiento = @FechaNacimiento,
    Ciudad = @Ciudad,
    IntencionBusqueda = @IntencionBusqueda,
    Genero = @Genero,
    FotoPerfil = @FotoPerfil,
    IdiomaPreferido = @IdiomaPreferido,
    TemaPreferido = @TemaPreferido,
    BioAI = @BioAI,
    EmbeddingPerfil = @EmbeddingPerfil,
    Estado = @Estado
WHERE IdUsuario = @IdUsuario;";

        public const string UsuarioUpdateWithoutPassword = @"
UPDATE [Usuario]
SET Nombre = @Nombre,
    Email = @Email,
    FechaNacimiento = @FechaNacimiento,
    Ciudad = @Ciudad,
    IntencionBusqueda = @IntencionBusqueda,
    Genero = @Genero,
    FotoPerfil = @FotoPerfil,
    IdiomaPreferido = @IdiomaPreferido,
    TemaPreferido = @TemaPreferido,
    BioAI = @BioAI,
    EmbeddingPerfil = @EmbeddingPerfil,
    Estado = @Estado
WHERE IdUsuario = @IdUsuario;";

        public const string UsuarioDelete = "DELETE FROM [Usuario] WHERE IdUsuario = @id;";

        public const string SesionSelectByToken = @"
SELECT IdSesion, IdUsuario, Token, ExpiraEn, CreadoEn
FROM [Sesion]
WHERE Token = @token;";

        public const string SesionInsert = @"
INSERT INTO [Sesion] (IdUsuario, Token, ExpiraEn, CreadoEn)
VALUES (@IdUsuario, @Token, @ExpiraEn, @CreadoEn);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string SesionDeleteByToken = "DELETE FROM [Sesion] WHERE Token = @token;";

        public const string MatchSelectAll = @"
SELECT IdMatch, IdUsuario1, IdUsuario2, Compatibilidad, ExplicacionAfinidad, FechaMatch, FechaActualizacion, SugerenciaIA, Estado
FROM [Match]
ORDER BY FechaMatch DESC;";

        public const string MatchSelectByUser = @"
SELECT IdMatch, IdUsuario1, IdUsuario2, Compatibilidad, ExplicacionAfinidad, FechaMatch, FechaActualizacion, SugerenciaIA, Estado
FROM [Match]
WHERE IdUsuario1 = @idUsuario OR IdUsuario2 = @idUsuario
ORDER BY FechaMatch DESC;";

        public const string MatchSelectById = @"
SELECT IdMatch, IdUsuario1, IdUsuario2, Compatibilidad, ExplicacionAfinidad, FechaMatch, FechaActualizacion, SugerenciaIA, Estado
FROM [Match]
WHERE IdMatch = @id;";

        public const string MatchInsert = @"
INSERT INTO [Match] (IdUsuario1, IdUsuario2, Compatibilidad, ExplicacionAfinidad, FechaMatch, FechaActualizacion, SugerenciaIA, Estado)
VALUES (@IdUsuario1, @IdUsuario2, @Compatibilidad, @ExplicacionAfinidad, @FechaMatch, @FechaActualizacion, @SugerenciaIA, @Estado);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string MatchUpdate = @"
UPDATE [Match]
SET IdUsuario1 = @IdUsuario1,
    IdUsuario2 = @IdUsuario2,
    Compatibilidad = @Compatibilidad,
    ExplicacionAfinidad = @ExplicacionAfinidad,
    FechaMatch = @FechaMatch,
    FechaActualizacion = @FechaActualizacion,
    SugerenciaIA = @SugerenciaIA,
    Estado = @Estado
WHERE IdMatch = @IdMatch;";

        public const string MatchUpdateEstado = @"
UPDATE [Match]
SET Estado = @estado,
    FechaActualizacion = @fechaActualizacion
WHERE IdMatch = @id;";

        public const string MatchDelete = "DELETE FROM [Match] WHERE IdMatch = @id;";

        public const string MensajeSelectByMatch = @"
SELECT IdMensaje, IdMatch, IdRemitente, IdDestinatario, [Mensaje] AS MensajeTexto, FechaMensaje, TipoMensaje, EstadoLeido
FROM [Mensaje]
WHERE IdMatch = @idMatch
ORDER BY FechaMensaje ASC;";

        public const string MensajeSelectConversation = @"
SELECT IdMensaje, IdMatch, IdRemitente, IdDestinatario, [Mensaje] AS MensajeTexto, FechaMensaje, TipoMensaje, EstadoLeido
FROM [Mensaje]
WHERE (IdRemitente = @idRemitente AND IdDestinatario = @idDestinatario)
   OR (IdRemitente = @idDestinatario AND IdDestinatario = @idRemitente)
ORDER BY FechaMensaje ASC;";

        public const string MensajeSelectById = @"
SELECT IdMensaje, IdMatch, IdRemitente, IdDestinatario, [Mensaje] AS MensajeTexto, FechaMensaje, TipoMensaje, EstadoLeido
FROM [Mensaje]
WHERE IdMensaje = @id;";

        public const string MensajeInsert = @"
INSERT INTO [Mensaje] (IdMatch, IdRemitente, IdDestinatario, [Mensaje], FechaMensaje, TipoMensaje, EstadoLeido)
VALUES (@IdMatch, @IdRemitente, @IdDestinatario, @MensajeTexto, @FechaMensaje, @TipoMensaje, @EstadoLeido);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string MensajeMarkAsRead = @"
UPDATE [Mensaje]
SET EstadoLeido = 1
WHERE IdMensaje = @id;";

        public const string MensajeSelectNoLeidos = @"
SELECT IdMensaje, IdMatch, IdRemitente, IdDestinatario, [Mensaje] AS MensajeTexto, FechaMensaje, TipoMensaje, EstadoLeido
FROM [Mensaje]
WHERE IdDestinatario = @idUsuario AND EstadoLeido = 0 AND Estado = 1;";

        public const string MensajeDelete = "DELETE FROM [Mensaje] WHERE IdMensaje = @id;";

        public const string PreguntasSelectAll = @"
SELECT IdPregunta, TextoPregunta, TextoPreguntaEN, Icono, Orden
FROM [PreguntaEncuesta]
ORDER BY Orden ASC;";

        public const string PreguntaSelectById = @"
SELECT IdPregunta, TextoPregunta, TextoPreguntaEN, Icono, Orden
FROM [PreguntaEncuesta]
WHERE IdPregunta = @id;";

        public const string PreguntaInsert = @"
INSERT INTO [PreguntaEncuesta] (TextoPregunta, TextoPreguntaEN, Icono, Orden)
VALUES (@TextoPregunta, @TextoPreguntaEN, @Icono, @Orden);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string PreguntaUpdate = @"
UPDATE [PreguntaEncuesta]
SET TextoPregunta = @TextoPregunta,
    TextoPreguntaEN = @TextoPreguntaEN,
    Icono = @Icono,
    Orden = @Orden
WHERE IdPregunta = @IdPregunta;";

        public const string PreguntaDelete = "DELETE FROM [PreguntaEncuesta] WHERE IdPregunta = @id;";

        public const string RespuestasSelectAll = @"
SELECT IdRespuesta, IdUsuario, IdPregunta, RespuestaTexto
FROM [RespuestaEncuesta]
ORDER BY IdRespuesta DESC;";

        public const string RespuestaSelectById = @"
SELECT IdRespuesta, IdUsuario, IdPregunta, RespuestaTexto
FROM [RespuestaEncuesta]
WHERE IdRespuesta = @id;";

        public const string RespuestasSelectByUsuario = @"
SELECT IdRespuesta, IdUsuario, IdPregunta, RespuestaTexto
FROM [RespuestaEncuesta]
WHERE IdUsuario = @idUsuario
ORDER BY IdPregunta ASC;";

        public const string RespuestaInsert = @"
INSERT INTO [RespuestaEncuesta] (IdUsuario, IdPregunta, RespuestaTexto)
VALUES (@IdUsuario, @IdPregunta, @RespuestaTexto);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string RespuestaUpdate = @"
UPDATE [RespuestaEncuesta]
SET IdUsuario = @IdUsuario,
    IdPregunta = @IdPregunta,
    RespuestaTexto = @RespuestaTexto
WHERE IdRespuesta = @IdRespuesta;";

        public const string RespuestaDelete = "DELETE FROM [RespuestaEncuesta] WHERE IdRespuesta = @id;";

        public const string IntencionesSelectAll = @"
SELECT IdIntencion, Nombre, NombreEN
FROM [IntencionBusqueda]
ORDER BY IdIntencion ASC;";

        public const string IntencionSelectById = @"
SELECT IdIntencion, Nombre, NombreEN
FROM [IntencionBusqueda]
WHERE IdIntencion = @id;";

        public const string IntencionInsert = @"
INSERT INTO [IntencionBusqueda] (Nombre, NombreEN)
VALUES (@Nombre, @NombreEN);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string IntencionUpdate = @"
UPDATE [IntencionBusqueda]
SET Nombre = @Nombre,
    NombreEN = @NombreEN
WHERE IdIntencion = @IdIntencion;";

        public const string IntencionDelete = "DELETE FROM [IntencionBusqueda] WHERE IdIntencion = @id;";
    }
}
