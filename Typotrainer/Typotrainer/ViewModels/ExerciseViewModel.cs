using Domain.Models;
using Domain.Services;
using Domain.Repositories;
using System.Diagnostics;

namespace Typotrainer.ViewModels
{
    /// <summary>
    /// Exercise ViewModel - handles typing exercise logic
    /// Behoudt de originele functionaliteit van PageOefening.xaml.cs
    /// </summary>
    public class ExerciseViewModel : BaseViewModel
    {
        private readonly ITypingService _typingService;
        private readonly ISentenceService _sentenceService;
        private readonly IExerciseRepository _exerciseRepository;

        private string _correctSentence = string.Empty;
        private string _typedText = string.Empty;
        private int _errorCount = 0;
        private HashSet<int> _errorPositions = new();
        private Stopwatch _stopwatch = new();
        private int _currentUserId = 1; // Demo user ID

        public string CorrectSentence
        {
            get => _correctSentence;
            set => SetProperty(ref _correctSentence, value);
        }

        public string TypedText
        {
            get => _typedText;
            set
            {
                SetProperty(ref _typedText, value);
                OnTextChanged(value);
            }
        }

        public int ErrorCount
        {
            get => _errorCount;
            set => SetProperty(ref _errorCount, value);
        }

        public string ErrorCountText => $"Fouten: {ErrorCount}";

        public ExerciseViewModel(
            ITypingService typingService,
            ISentenceService sentenceService,
            IExerciseRepository exerciseRepository)
        {
            _typingService = typingService;
            _sentenceService = sentenceService;
            _exerciseRepository = exerciseRepository;
        }

        public void StartExercise()
        {
            // Reset alles
            ErrorCount = 0;
            _errorPositions.Clear();
            TypedText = string.Empty;

            // Haal random zin op
            CorrectSentence = _sentenceService.GetRandomSentence(Domain.Models.Difficulty.Easy);

            // Start timer
            _stopwatch.Restart();
        }

        private async void OnTextChanged(string newText)
        {
            if (string.IsNullOrWhiteSpace(CorrectSentence))
                return;

            // Check elke letter
            for (int i = 0; i < newText.Length; i++)
            {
                char typedChar = newText[i];
                bool correct = _typingService.IsCorrectLetter(CorrectSentence, i, typedChar);

                if (!correct && i < CorrectSentence.Length)
                {
                    if (!_errorPositions.Contains(i))
                    {
                        _errorPositions.Add(i);
                        ErrorCount++;
                        OnPropertyChanged(nameof(ErrorCountText));
                    }
                }
            }

            // Check of oefening compleet is
            if (newText.Length == CorrectSentence.Length)
            {
                _stopwatch.Stop();

                Debug.WriteLine($"Oefening voltooid! Fouten: {ErrorCount}");

                // Sla resultaat op in database
                await SaveExerciseResult();

                // Start nieuwe oefening
                StartExercise();
            }
        }

        private async Task SaveExerciseResult()
        {
            var exercise = new Exercise(
                _currentUserId,
                CorrectSentence,
                Domain.Models.Difficulty.Easy
            )
            {
                ErrorCount = ErrorCount,
                Duration = _stopwatch.Elapsed
            };

            // Bereken statistieken
            exercise.CalculateAccuracy(CorrectSentence.Length);
            exercise.CalculateWPM(CorrectSentence.Length);

            // Sla op in database
            await _exerciseRepository.AddAsync(exercise);
        }

        public FormattedString GetColoredText()
        {
            var formatted = new FormattedString();

            for (int i = 0; i < TypedText.Length; i++)
            {
                char typedChar = TypedText[i];
                bool correct = _typingService.IsCorrectLetter(CorrectSentence, i, typedChar);

                formatted.Spans.Add(new Span
                {
                    Text = typedChar.ToString(),
                    TextColor = correct ? Colors.Green : Colors.Red
                });
            }

            return formatted;
        }
    }
}