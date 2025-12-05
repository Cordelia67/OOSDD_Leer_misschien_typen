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

    public PageOefening()
    {
        InitializeComponent();
        _typingService = new TypingService();
        _sentenceService = new SentenceService();

        // ? Nog geen random zin ophalen bij opstart
        correctZin = "";

        // Placeholder tekst tonen
        CorrectText.Text = "Klik op start oefening om te starten.";
        FoutenCount.Text = $"Fouten: {AantalFouten}";

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(1);
        _timer.Tick += Timer_Tick;
    }

    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Als oefening nog niet gestart is ? niets doen
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
                    FoutenCount.Text = $"Fouten: {AantalFouten}";
                }
            }

            formatted.Spans.Add(new Span
            {
                Text = typedChar.ToString(),
                TextColor = correct ? Colors.Green : Colors.Red
            });
        }

        ColoredOutput.FormattedText = formatted;

        if (typed.Length == correctZin.Length)
        {
            Debug.WriteLine($"Oefening voltooid! Aantal fouten: {AantalFouten}");

            correctZin = _sentenceService.GetRandomSentence(Difficulty.Easy);
            CorrectText.Text = correctZin;

            foutPosities.Clear();

            InputEditor.Text = "";
            ColoredOutput.FormattedText = new FormattedString();
            InputEditor.Focus();
        }
    }

    private async void Startknop_Clicked(object sender, EventArgs e)
    {
        // Reset
        AantalFouten = 0;
        foutPosities.Clear();
        FoutenCount.Text = $"Fouten: {AantalFouten}";

        InputEditor.Text = "";
        ColoredOutput.FormattedText = new FormattedString();

        //Hier wordt nu pas de random zin opgehaald
        correctZin = _sentenceService.GetRandomSentence(Difficulty.Easy);
        CorrectText.Text = correctZin;

        InputEditor.IsVisible = true;
        await Task.Delay(50);
        InputEditor.Focus();

        _stopwatch.Restart();
        _timer.Start();
        _timerRunning = true;
    }
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _timerRunning = false;
    private IDispatcherTimer _timer;

    private void Timer_Tick(object sender, EventArgs e)
    {
        TimerLabel.Text = $"Tijd: {_stopwatch.Elapsed.TotalSeconds:0.000}";
    }
}
