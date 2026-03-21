using Synchro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synchro.Infrastructure.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByEmailAsync(string email);
        Task<int> InsertAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
    }
}
