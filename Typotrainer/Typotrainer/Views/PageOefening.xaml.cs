using Typotrainer.Services;
using System.Diagnostics;

namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
    private readonly TypingService _typingService;
    private readonly SentenceService _sentenceService;

    private string correctZin;
    private int AantalFouten = 0;
    private int TotaalGetypt = 0;
    private double GemiddeldeAccuracy = 0.0;

    private HashSet<int> geteldePosities = new();
    private HashSet<int> foutPosities = new();

    private Difficulty selectedDifficulty = Difficulty.Easy;

    public PageOefening()
    {
        InitializeComponent();
        _typingService = new TypingService();
        _sentenceService = new SentenceService();

        correctZin = "";
        CorrectText.Text = "Kies een moeilijkheid en klik op start oefening om te starten.";

        FoutenCount.Text = $"Fouten: {AantalFouten}";
        TotaalCount.Text = $"Totaal: {TotaalGetypt}";

        DifficultyPicker.SelectedIndex = 0;
        DifficultyPicker.SelectedIndexChanged += DifficultyPicker_SelectedIndexChanged;
    }

    private void DifficultyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DifficultyPicker.SelectedIndex >= 0)
        {
            if (Enum.TryParse(DifficultyPicker.SelectedItem.ToString(), out Difficulty diff))
            {
                selectedDifficulty = diff;
                Debug.WriteLine($"Moeilijkheid: {selectedDifficulty}");
            }
        }
    }

    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(correctZin))
            return;

        string typed = InputEditor.Text ?? "";
        var formatted = new FormattedString();

        for (int i = 0; i < typed.Length; i++)
        {
            char typedChar = typed[i];

            // Tel unieke typeposities
            if (!geteldePosities.Contains(i))
            {
                TotaalGetypt++;
                geteldePosities.Add(i);
                TotaalCount.Text = $"Totaal: {TotaalGetypt}";
            }

            bool correct = _typingService.IsCorrectLetter(correctZin, i, typedChar);

            // Foutentelling
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

        // --- ACCURACY BEREKENING ---
        if (TotaalGetypt > 0)
        {
            int correctAantal = TotaalGetypt - AantalFouten;
            GemiddeldeAccuracy = (double)correctAantal / TotaalGetypt * 100.0;
            Nauwkeurigheid.Text = GemiddeldeAccuracy.ToString("F2") + "%";
        }
        else
        {
            Nauwkeurigheid.Text = "0%";
        }

        // --- VOLGENDE ZIN ---
        if (typed.Length == correctZin.Length)
        {
            Debug.WriteLine($"Oefening voltooid! Fouten: {AantalFouten}");

            correctZin = _sentenceService.GetRandomSentence(selectedDifficulty);
            CorrectText.Text = correctZin;

            foutPosities.Clear();
            geteldePosities.Clear();

            InputEditor.Text = "";
            ColoredOutput.FormattedText = new FormattedString();
            InputEditor.Focus();
        }
    }

    private async void Startknop_Clicked(object sender, EventArgs e)
    {
        AantalFouten = 0;
        TotaalGetypt = 0;
        geteldePosities.Clear();
        foutPosities.Clear();

        FoutenCount.Text = $"Fouten: {AantalFouten}";
        TotaalCount.Text = $"Totaal: {TotaalGetypt}";
        Nauwkeurigheid.Text = "0%";

        InputEditor.Text = "";
        ColoredOutput.FormattedText = new FormattedString();

        correctZin = _sentenceService.GetRandomSentence(selectedDifficulty);
        CorrectText.Text = correctZin;

        InputEditor.IsVisible = true;
        await Task.Delay(50);
        InputEditor.Focus();
    }
}
