using Typotrainer.Services;
using System.Diagnostics;

namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
    private readonly TypingService _typingService;
    private readonly SentenceService _sentenceService;
    private string correctZin;
    private int AantalFouten = 0;
    private HashSet<int> foutPosities = new();
    private int totalCharactersTyped = 0;

    // Timer velden
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _timerRunning = false;
    private IDispatcherTimer _timer;

    public PageOefening()
    {
        InitializeComponent();
        _typingService = new TypingService();
        _sentenceService = new SentenceService();

        // No sentence yet
        correctZin = "";

        // Placeholder text
        CorrectText.Text = "Klik op start oefening om te starten.";
        UpdateStats();

        // Setup timer
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100); // Update every 100ms
        _timer.Tick += Timer_Tick;
    }

    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        // If exercise hasn't started, do nothing
        if (string.IsNullOrWhiteSpace(correctZin))
            return;

        string typed = InputEditor.Text ?? "";
        var formatted = new FormattedString();

        for (int i = 0; i < typed.Length; i++)
        {
            char typedChar = typed[i];
            bool correct = _typingService.IsCorrectLetter(correctZin, i, typedChar);

            if (!correct && i < correctZin.Length)
            {
                if (!foutPosities.Contains(i))
                {
                    AantalFouten++;
                    foutPosities.Add(i);
                }
            }

            formatted.Spans.Add(new Span
            {
                Text = typedChar.ToString(),
                TextColor = correct ? Colors.Green : Colors.Red
            });
        }

        ColoredOutput.FormattedText = formatted;

        // Update statistieken
        UpdateStats();

        // Check of zin voltooid is
        if (typed.Length == correctZin.Length)
        {
            Debug.WriteLine($"Oefening voltooid! Aantal fouten: {AantalFouten}");

            // Kijk totaal aantal karakters getypd bij alle zinnen
            totalCharactersTyped += correctZin.Length;

            // Pak nieuwe zin
            correctZin = _sentenceService.GetRandomSentence(Difficulty.Easy);
            CorrectText.Text = correctZin;

            // Laat timer nog steeds door gaan zelfs na errors
            foutPosities.Clear();

            InputEditor.Text = "";
            ColoredOutput.FormattedText = new FormattedString();
            InputEditor.Focus();
        }
    }

    private async void Startknop_Clicked(object sender, EventArgs e)
    {
        // Reset everything
        AantalFouten = 0;
        foutPosities.Clear();
        totalCharactersTyped = 0;

        InputEditor.Text = "";
        ColoredOutput.FormattedText = new FormattedString();

        // Pak eerste zin
        correctZin = _sentenceService.GetRandomSentence(Difficulty.Easy);
        CorrectText.Text = correctZin;

        InputEditor.IsVisible = true;
        await Task.Delay(50);
        InputEditor.Focus();

        // Start timer
        _stopwatch.Restart();
        _timer.Start();
        _timerRunning = true;

        UpdateStats();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        // Calculeer hoeveelheid tijd
        double elapsedMinutes = _stopwatch.Elapsed.TotalMinutes;

        // Calculeer WPM (Woorden Per Minuut)
        // Standaard: 1 woord = 5 karakters
        int currentTypedChars = (InputEditor.Text?.Length ?? 0) + totalCharactersTyped;
        double words = currentTypedChars / 5.0;
        double wpm = elapsedMinutes > 0 ? words / elapsedMinutes : 0;

        // Calculate accuracy
        int totalCharsAttempted = currentTypedChars;
        double accuracy = totalCharsAttempted > 0
            ? ((totalCharsAttempted - AantalFouten) / (double)totalCharsAttempted) * 100
            : 100;

        // Update waardes
        SnelheidLabel.Text = $"Snelheid:\n{wpm:0} WPM";
        NauwkeurigheidLabel.Text = $"Nauwkeurigheid:\n{accuracy:0.0}%";
        TimerLabel.Text = $"Tijd:\n{_stopwatch.Elapsed.Minutes:00}:{_stopwatch.Elapsed.Seconds:00}";
        FoutenCount.Text = $"Fouten: {AantalFouten}";
    }
}