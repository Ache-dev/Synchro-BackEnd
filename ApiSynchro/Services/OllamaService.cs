using System.Text;
using System.Text.Json;

namespace ApiSynchro.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;

        public OllamaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:11434/");
        }

        public async Task<string> GenerarBioAsync(string nombre, string intereses, string descripcionBase)
        {
            var prompt = $"Actúa como un experto en crear perfiles atractivos para apps de citas. Eres directo, original y usas un tono ligeramente coqueto pero respetuoso en español. Crea una biografía de máximo 3 líneas para el siguiente perfil:\nNombre: {nombre}\nIntereses: {intereses}\nDescripción base: {descripcionBase}\nSolo responde con la biografía, sin explicaciones adicionales.";

            var requestBody = new
            {
                model = "llama3.2", // o el modelo que se use
                prompt = prompt,
                stream = false
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/generate", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
                if (result.TryGetProperty("response", out var bioText))
                {
                    return bioText.GetString()?.Trim() ?? string.Empty;
                }
            }

            return descripcionBase; // Fallback
        }
    }
}
