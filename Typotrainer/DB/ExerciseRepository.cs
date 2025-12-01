using Domain.Models;
using Domain.Repositories;

namespace DB
{
    /// <summary>
    /// In-Memory Exercise Repository - TIJDELIJK zonder database
    /// Data blijft bestaan zolang app draait, wordt gewist bij afsluiten
    /// Later te vervangen door echte database implementatie
    /// </summary>
    public class ExerciseRepository : IExerciseRepository
    {
        // Tijdelijke opslag in geheugen
        private static readonly List<Exercise> _exercises = new();
        private static int _nextId = 1;

        public Task AddAsync(Exercise exercise, CancellationToken ct = default)
        {
            // Zet ID en timestamp
            exercise.GetType().GetProperty("Id")?.SetValue(exercise, _nextId++);
            exercise.GetType().GetProperty("CompletedAt")?.SetValue(exercise, DateTime.UtcNow);

            // Voeg toe aan lijst
            _exercises.Add(exercise);

            return Task.CompletedTask;
        }

        public Task<Exercise?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var exercise = _exercises.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(exercise);
        }

        public Task<List<Exercise>> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            var exercises = _exercises
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CompletedAt)
                .ToList();
            return Task.FromResult(exercises);
        }

        public Task<List<Exercise>> GetByUserIdAndDifficultyAsync(int userId, Difficulty difficulty, CancellationToken ct = default)
        {
            var exercises = _exercises
                .Where(e => e.UserId == userId && e.DifficultyLevel == difficulty)
                .OrderByDescending(e => e.CompletedAt)
                .ToList();
            return Task.FromResult(exercises);
        }

        public Task<double> GetAverageWPMAsync(int userId, CancellationToken ct = default)
        {
            var userExercises = _exercises.Where(e => e.UserId == userId).ToList();

            if (!userExercises.Any())
                return Task.FromResult(0.0);

            var average = userExercises.Average(e => e.WordsPerMinute);
            return Task.FromResult(average);
        }

        public Task<double> GetAverageAccuracyAsync(int userId, CancellationToken ct = default)
        {
            var userExercises = _exercises.Where(e => e.UserId == userId).ToList();

            if (!userExercises.Any())
                return Task.FromResult(0.0);

            var average = userExercises.Average(e => e.Accuracy);
            return Task.FromResult(average);
        }

        public Task<List<Exercise>> ListAsync(CancellationToken ct = default)
        {
            return Task.FromResult(_exercises.OrderByDescending(e => e.CompletedAt).ToList());
        }

        public Task UpdateAsync(Exercise exercise, CancellationToken ct = default)
        {
            var existing = _exercises.FirstOrDefault(e => e.Id == exercise.Id);
            if (existing != null)
            {
                existing.ErrorCount = exercise.ErrorCount;
                existing.WordsPerMinute = exercise.WordsPerMinute;
                existing.Accuracy = exercise.Accuracy;
                existing.Duration = exercise.Duration;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var exercise = _exercises.FirstOrDefault(e => e.Id == id);
            if (exercise != null)
            {
                _exercises.Remove(exercise);
            }
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }
}