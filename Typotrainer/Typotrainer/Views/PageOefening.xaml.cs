using Typotrainer.Core.Services;
using Typotrainer.Core.Models;
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
    private int voltooideOefeningen = 0;
    private const int maxOefeningen = 10;
    private bool isPaused = false;

    // Timer velden
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _timerRunning = false;
    private IDispatcherTimer _timer;

    // Constructor met dependency injection. Is mogelijk door registratie in MauiProgram.cs en MainPage.xaml.cs
    public PageOefening(TypingService typingService, SentenceService sentenceService)
    {
        InitializeComponent();

        // Gebruik injecteerde services
        _typingService = typingService;
        _sentenceService = sentenceService;

        // Lege zin totdat oefening is gestart
        correctZin = "";

        // Placeholder text
        CorrectText.Text = "Klik op start oefening om te starten.";
        UpdateStats();

        // Setup timer
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += Timer_Tick;
    }
    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        // If exercise hasn't started or is paused, do nothing
        if (string.IsNullOrWhiteSpace(correctZin) || isPaused)
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
            voltooideOefeningen++;

            // Check of we 10 oefeningen hebben voltooid
            if (voltooideOefeningen >= maxOefeningen)
            {
                OefeningenCompleet();
                return;
            }

            // Pak nieuwe zin
            correctZin = _sentenceService.GetRandomSentence(Difficulty.Easy);
            CorrectText.Text = correctZin;

            // Laat timer nog steeds door gaan zelfs na errors
            foutPosities.Clear();

            InputEditor.Text = "";
            ColoredOutput.FormattedText = new FormattedString();
            InputEditor.Focus();

            UpdateStats();
        }
    }

    private async void Startknop_Clicked(object sender, EventArgs e)
    {
        // Check of we hervatten na pauze
        if (isPaused)
        {
            HervattOefening();
            return;
        }

        // Reset alle waardes
        AantalFouten = 0;
        foutPosities.Clear();
        totalCharactersTyped = 0;
        voltooideOefeningen = 0;
        isPaused = false;

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

        // Update knoppen
        StartButton.IsVisible = false;
        PauseButton.IsVisible = true;
        PauseButton.Text = "Pauzeer";

        UpdateStats();
    }

    private void Pauzeknop_Clicked(object sender, EventArgs e)
    {
        if (isPaused)
        {
            HervattOefening();
        }
        else
        {
            PauzeerOefening();
        }
    }

    private void PauzeerOefening()
    {
        isPaused = true;
        _stopwatch.Stop();
        _timer.Stop();

        // Disable invoer
        InputEditor.IsEnabled = false;

        // Update knoppen
        PauseButton.Text = "Hervat";
        CorrectText.Text = "Oefening gepauzeerd. Klik op 'Hervat' om door te gaan.";
    }

    private async void HervattOefening()
    {
        isPaused = false;
        _stopwatch.Start();
        _timer.Start();

        // Enable invoer
        InputEditor.IsEnabled = true;
        CorrectText.Text = correctZin;

        // Update knoppen
        PauseButton.Text = "Pauzeer";

        await Task.Delay(50);
        InputEditor.Focus();
    }

    private void OefeningenCompleet()
    {
        // Stop timer
        _stopwatch.Stop();
        _timer.Stop();
        _timerRunning = false;

        // Disable invoer
        InputEditor.IsEnabled = false;

        // Toon voltooiingsbericht
        CorrectText.Text = "Gefeliciteerd! Je hebt 10 oefeningen voltooid!";
        ColoredOutput.FormattedText = new FormattedString();

        // Update knoppen
        StartButton.IsVisible = true;
        StartButton.Text = "Start nieuwe reeks";
        PauseButton.IsVisible = false;

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
        OefeningenCount.Text = $"Oefeningen: {voltooideOefeningen}/{maxOefeningen}";
    }
}