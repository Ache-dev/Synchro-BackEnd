using ApiSynchro.Models;

namespace ApiSynchro.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEn { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
