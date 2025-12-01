using Domain.Repositories;

namespace Typotrainer.ViewModels
{
    /// <summary>
    /// Dashboard ViewModel - shows user statistics
    /// </summary>
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IExerciseRepository _exerciseRepository;
        private int _currentUserId = 1; // Demo user ID

        private double _averageWPM;
        private double _averageAccuracy;
        private int _totalExercises;

        public double AverageWPM
        {
            get => _averageWPM;
            set => SetProperty(ref _averageWPM, value);
        }

        public double AverageAccuracy
        {
            get => _averageAccuracy;
            set => SetProperty(ref _averageAccuracy, value);
        }

        public int TotalExercises
        {
            get => _totalExercises;
            set => SetProperty(ref _totalExercises, value);
        }

        public DashboardViewModel(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task LoadStatisticsAsync()
        {
            // Haal statistieken op uit database
            AverageWPM = await _exerciseRepository.GetAverageWPMAsync(_currentUserId);
            AverageAccuracy = await _exerciseRepository.GetAverageAccuracyAsync(_currentUserId);

            var exercises = await _exerciseRepository.GetByUserIdAsync(_currentUserId);
            TotalExercises = exercises.Count;
        }
    }
}