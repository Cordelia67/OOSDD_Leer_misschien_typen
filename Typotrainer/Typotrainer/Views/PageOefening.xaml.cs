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
    private Difficulty selectedDifficulty = Difficulty.Easy;  // Standaard moeilijkheid

    public PageOefening()
    {
        InitializeComponent();
        _typingService = new TypingService();
        _sentenceService = new SentenceService();

        // ? Nog geen random zin ophalen bij opstart
        correctZin = "";

        // Placeholder tekst tonen
        CorrectText.Text = "Kies een moeilijkheid en klik op start oefening om te starten.";
        FoutenCount.Text = $"Fouten: {AantalFouten}";

        // Zet de standaard geselecteerde moeilijkheid
        DifficultyPicker.SelectedIndex = 0; // Easy is de eerste optie

        // Voeg event handler toe voor wanneer de moeilijkheid verandert
        DifficultyPicker.SelectedIndexChanged += DifficultyPicker_SelectedIndexChanged;
    }

    private void DifficultyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Update de geselecteerde moeilijkheid wanneer de gebruiker een keuze maakt
        if (DifficultyPicker.SelectedIndex >= 0)
        {
            string selected = DifficultyPicker.SelectedItem.ToString();
            if (Enum.TryParse<Difficulty>(selected, out Difficulty difficulty))
            {
                selectedDifficulty = difficulty;
                Debug.WriteLine($"Moeilijkheid gewijzigd naar: {selectedDifficulty}");
            }
        }
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

            // Gebruik de geselecteerde moeilijkheid voor de volgende zin
            correctZin = _sentenceService.GetRandomSentence(selectedDifficulty);
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

        // Gebruik de geselecteerde moeilijkheid
        correctZin = _sentenceService.GetRandomSentence(selectedDifficulty);
        CorrectText.Text = correctZin;

        InputEditor.IsVisible = true;
        await Task.Delay(50);
        InputEditor.Focus();
    }
}