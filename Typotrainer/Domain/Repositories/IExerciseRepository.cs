using Domain.Models;

namespace Domain.Repositories
{
    /// <summary>
    /// Exercise repository interface - exercise-specific operations
    /// </summary>
    public interface IExerciseRepository : IRepository<Exercise>
    {
        Task<List<Exercise>> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<List<Exercise>> GetByUserIdAndDifficultyAsync(int userId, Difficulty difficulty, CancellationToken ct = default);
        Task<double> GetAverageWPMAsync(int userId, CancellationToken ct = default);
        Task<double> GetAverageAccuracyAsync(int userId, CancellationToken ct = default);
    }
}