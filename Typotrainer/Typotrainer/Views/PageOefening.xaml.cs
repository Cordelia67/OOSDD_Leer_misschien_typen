using Typotrainer.Services;
using System.Diagnostics;
namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
    private readonly TypingService _typingService;
    private readonly SentenceService _sentenceService;
    private string correctZin;
    private int AantalFouten = 0;
    private HashSet<int> foutPosities = new();        // ← Om dubbele telling te voorkomen

    public PageOefening()
    {
        InitializeComponent();
        _typingService = new TypingService();
        _sentenceService = new SentenceService();

        correctZin = _sentenceService.GetRandomSentence(Difficulty.Easy);

        CorrectText.Text = correctZin;
        FoutenCount.Text = $"Fouten: {AantalFouten}";
    }

    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        string typed = InputEditor.Text ?? "";
        var formatted = new FormattedString();

        for (int i = 0; i < typed.Length; i++)
        {
            char typedChar = typed[i];

            bool correct = _typingService.IsCorrectLetter(correctZin, i, typedChar);


            // telt fout als deze fout nog niet eerder is geteld
            if (!correct && i < correctZin.Length)
            {
                if (!foutPosities.Contains(i))
                {
                    AantalFouten++;
                    foutPosities.Add(i);
                    FoutenCount.Text = $"Fouten: {AantalFouten}";   // ← UI bijwerken
                }
            }

            formatted.Spans.Add(new Span
            {
                Text = typedChar.ToString(),
                TextColor = correct ? Colors.Green : Colors.Red
            });
        }

        ColoredOutput.FormattedText = formatted;

        // Controleert of de zin volledig is getypt
        if (typed.Length == correctZin.Length)
        {
            //debug line om fouten telling te controleren
            Debug.WriteLine($"Oefening voltooid! Aantal fouten: {AantalFouten}");
            // Nieuwe zin instellen later vervangen met zin select logica
            correctZin = _sentenceService.GetRandomSentence(Difficulty.Easy);
            CorrectText.Text = correctZin;

            // Reset foutlocaties maar behoud totaal aantal fouten
            foutPosities.Clear();

            // Editor en gekleurde output resetten
            InputEditor.Text = "";
            ColoredOutput.FormattedText = new FormattedString();

            // Focus teruggeven
            InputEditor.Focus();
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Reset
        InputEditor.Text = "";
        ColoredOutput.FormattedText = new FormattedString();

        // Editor tonen zodat hij focus mag krijgen
        InputEditor.IsVisible = true;

        // Kleine delay zodat MAUI de editor kan tonen
        await Task.Delay(50);

        // Focus erzéker geven
        InputEditor.Focus();
    }
}
