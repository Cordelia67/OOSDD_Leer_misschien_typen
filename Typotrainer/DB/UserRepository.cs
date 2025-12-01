using Domain.Models;
using Domain.Repositories;

namespace DB
{
    /// <summary>
    /// In-Memory User Repository - TIJDELIJK zonder database
    /// Data blijft bestaan zolang app draait, wordt gewist bij afsluiten
    /// Later te vervangen door echte database implementatie
    /// </summary>
    public class UserRepository : IUserRepository
    {
        // Tijdelijke opslag in geheugen
        private static readonly List<User> _users = new();
        private static int _nextId = 1;

        // Demo users voor testen
        static UserRepository()
        {
            // Voeg demo users toe
            var demoUser1 = new User("jan@voorbeeld.nl", "wachtwoord123");
            _users.Add(demoUser1);
            demoUser1.GetType().GetProperty("Id")?.SetValue(demoUser1, _nextId++);

            var demoUser2 = new User("marie@voorbeeld.nl", "demo1234");
            _users.Add(demoUser2);
            demoUser2.GetType().GetProperty("Id")?.SetValue(demoUser2, _nextId++);

            var demoUser3 = new User("admin@typotrainer.nl", "admin");
            _users.Add(demoUser3);
            demoUser3.GetType().GetProperty("Id")?.SetValue(demoUser3, _nextId++);
        }

        public Task AddAsync(User user, CancellationToken ct = default)
        {
            // Zet ID
            user.GetType().GetProperty("Id")?.SetValue(user, _nextId++);

            // Voeg toe aan lijst
            _users.Add(user);

            return Task.CompletedTask;
        }

        public Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = _users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            var exists = _users.Any(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task<List<User>> ListAsync(CancellationToken ct = default)
        {
            return Task.FromResult(_users.ToList());
        }

        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.Email = user.Email;
                existing.PasswordHash = user.PasswordHash;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
            }
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }
}