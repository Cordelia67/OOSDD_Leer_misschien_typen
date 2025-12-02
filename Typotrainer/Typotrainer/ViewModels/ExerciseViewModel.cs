using Domain.Models;
using Domain.Services;
using Domain.Repositories;
using System.Diagnostics;

namespace Typotrainer.ViewModels
{
    /// <summary>
    /// ViewModel voor typ oefeningen
    /// Handles alle logica voor het oefenen van typen
    /// </summary>
    public class ExerciseViewModel : BaseViewModel
    {
        private const int CharactersPerWord = 5;
        private const int DefaultUserId = 1;

        private readonly ITypingService _typingService;
        private readonly ISentenceService _sentenceService;
        private readonly IExerciseRepository _exerciseRepository;

        private string _correctSentence = string.Empty;
        private string _typedText = string.Empty;
        private int _errorCount = 0;
        private readonly HashSet<int> _errorPositions = new();
        private readonly Stopwatch _stopwatch = new();

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
                ProcessTypedText(value);
            }
        }

        public int ErrorCount
        {
            get => _errorCount;
            set => SetProperty(ref _errorCount, value);
        }

        public string ErrorCountText => $"Fouten: {ErrorCount}";

        public event EventHandler? ExerciseCompleted;

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
            ResetExercise();
            LoadNewSentence();
            StartTimer();
        }

        public FormattedString GetColoredText()
        {
            var formatted = new FormattedString();

            for (int i = 0; i < TypedText.Length; i++)
            {
                formatted.Spans.Add(CreateColoredSpan(i));
            }

            return formatted;
        }

        private void ResetExercise()
        {
            ErrorCount = 0;
            _errorPositions.Clear();
            TypedText = string.Empty;
        }

        private void LoadNewSentence()
        {
            CorrectSentence = _sentenceService.GetRandomSentence(Difficulty.Easy);
        }

        private void StartTimer()
        {
            _stopwatch.Restart();
        }

        private void ProcessTypedText(string text)
        {
            if (string.IsNullOrWhiteSpace(CorrectSentence))
                return;

            if (text == null)
                return;

            CheckForErrors(text);

            if (IsExerciseComplete(text))
            {
                CompleteExercise();
            }
        }

        private void CheckForErrors(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (IsNewError(i, text[i]))
                {
                    RegisterError(i);
                }
            }
        }

        private bool IsNewError(int index, char typedChar)
        {
            return !_typingService.IsCorrectLetter(CorrectSentence, index, typedChar)
                   && !_errorPositions.Contains(index)
                   && index < CorrectSentence.Length;
        }

        private void RegisterError(int position)
        {
            _errorPositions.Add(position);
            ErrorCount++;
            OnPropertyChanged(nameof(ErrorCountText));
        }

        private bool IsExerciseComplete(string text)
        {
            return text.Length == CorrectSentence.Length;
        }

        private async void CompleteExercise()
        {
            _stopwatch.Stop();
            await SaveResults();

            // Trigger event voor UI update
            ExerciseCompleted?.Invoke(this, EventArgs.Empty);

            // Geef UI tijd om te updaten voordat we een nieuwe oefening starten
            await Task.Delay(1500);
            StartExercise();
        }

        private async Task SaveResults()
        {
            var exercise = CreateExercise();
            CalculateStatistics(exercise);
            await _exerciseRepository.AddAsync(exercise);

            Debug.WriteLine($"Oefening voltooid! WPM: {exercise.WordsPerMinute}, " +
                          $"Accuracy: {exercise.Accuracy:F1}%, Fouten: {ErrorCount}");
        }

        private Exercise CreateExercise()
        {
            return new Exercise(DefaultUserId, CorrectSentence, Difficulty.Easy)
            {
                ErrorCount = ErrorCount,
                Duration = _stopwatch.Elapsed
            };
        }

        private void CalculateStatistics(Exercise exercise)
        {
            int totalChars = CorrectSentence.Length;
            exercise.CalculateAccuracy(totalChars);
            exercise.CalculateWPM(totalChars);
        }

        private Span CreateColoredSpan(int index)
        {
            char typedChar = TypedText[index];
            bool isCorrect = _typingService.IsCorrectLetter(CorrectSentence, index, typedChar);

            return new Span
            {
                Text = typedChar.ToString(),
                TextColor = isCorrect ? Colors.Green : Colors.Red
            };
        }
    }
}