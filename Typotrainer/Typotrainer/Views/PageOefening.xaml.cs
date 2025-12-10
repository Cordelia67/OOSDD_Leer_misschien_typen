using Typotrainer.Services;
using System.Diagnostics;

namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
    private readonly TypingService _typingService;
    private readonly SentenceService _sentenceService;

    private string correctZin;
    private Difficulty selectedDifficulty = Difficulty.Easy;

    private bool _isResetting = false;

    public PageOefening()
    {
        InitializeComponent();
        _typingService = new TypingService();
        _sentenceService = new SentenceService();

        correctZin = "";
        CorrectText.Text = "Kies een moeilijkheid en klik op start oefening om te starten.";

        DifficultyPicker.SelectedIndex = 0;
        DifficultyPicker.SelectedIndexChanged += DifficultyPicker_SelectedIndexChanged;
    }

    private void DifficultyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DifficultyPicker.SelectedIndex >= 0 &&
            Enum.TryParse(DifficultyPicker.SelectedItem.ToString(), out Difficulty diff))
        {
            selectedDifficulty = diff;
        }
    }

    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isResetting || string.IsNullOrWhiteSpace(correctZin))
            return;

        string typed = InputEditor.Text ?? "";
        var formatted = new FormattedString();

        // Alleen kleur per karakter, geen tel-logica meer
        for (int i = 0; i < typed.Length; i++)
        {
            char typedChar = typed[i];
            bool correct = (i < correctZin.Length && typedChar == correctZin[i]);

            formatted.Spans.Add(new Span
            {
                Text = typedChar.ToString(),
                TextColor = correct ? Colors.Green : Colors.Red
            });
        }

        ColoredOutput.FormattedText = formatted;

        // Volgende zin als de lengte gelijk is
        if (typed.Length == correctZin.Length)
        {
            correctZin = _sentenceService.GetRandomSentence(selectedDifficulty);
            CorrectText.Text = correctZin;

            _isResetting = true;
            InputEditor.Text = "";
            _isResetting = false;

            ColoredOutput.FormattedText = new FormattedString();
            InputEditor.Focus();
        }
    }

    private async void Startknop_Clicked(object sender, EventArgs e)
    {
        _isResetting = true;
        InputEditor.Text = "";
        _isResetting = false;

        ColoredOutput.FormattedText = new FormattedString();

        correctZin = _sentenceService.GetRandomSentence(selectedDifficulty);
        CorrectText.Text = correctZin;

        InputEditor.IsVisible = true;
        await Task.Delay(50);
        InputEditor.Focus();
    }
}
