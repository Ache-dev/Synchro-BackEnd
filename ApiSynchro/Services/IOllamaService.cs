namespace ApiSynchro.Services
{
    public interface IOllamaService
    {
        Task<string> GenerarBioAsync(string nombre, string intereses, string descripcionBase);
    }
}
