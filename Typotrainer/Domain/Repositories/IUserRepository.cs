using Domain.Models;

namespace Domain.Repositories
{
    /// <summary>
    /// User repository interface - user-specific operations
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    }
}