using System;
using System.Linq;

namespace ApiSynchro.Utils
{
    /// <summary>
    /// Proporciona utilidades matemáticas para el cálculo de afinidad entre embeddings.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Calcula la similitud coseno entre dos vectores de igual dimensión.
        /// </summary>
        /// <param name="vector1">Primer vector.</param>
        /// <param name="vector2">Segundo vector.</param>
        /// <returns>Valor entre -1 y 1 que representa la similitud.</returns>
        /// <exception cref="ArgumentException">Se produce cuando los vectores tienen distinta longitud.</exception>
        public static double CosineSimilarity(float[] vector1, float[] vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Vectors must have the same length");
            }

            double dotProduct = 0;
            double magnitude1 = 0;
            double magnitude2 = 0;

            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                magnitude1 += vector1[i] * vector1[i];
                magnitude2 += vector2[i] * vector2[i];
            }

            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0;
            }

            return dotProduct / (magnitude1 * magnitude2);
        }

        /// <summary>
        /// Convierte un embedding serializado en formato de arreglo JSON a un vector de tipo <see cref="float"/>.
        /// </summary>
        /// <param name="embedding">Embedding serializado, por ejemplo: <c>[1,2,3]</c>.</param>
        /// <returns>Vector de valores o arreglo vacío si no puede parsearse.</returns>
        public static float[] ParseEmbedding(string embedding)
        {
            if (string.IsNullOrWhiteSpace(embedding))
            {
                return Array.Empty<float>();
            }

            try
            {
                return embedding.Trim('[', ']')
                                .Split(',')
                                .Select(s => float.Parse(s.Trim()))
                                .ToArray();
            }
            catch
            {
                return Array.Empty<float>();
            }
        }
    }
}
