using Typotrainer.Services;

namespace Typotrainer.Views;

public partial class PageResultaten : ContentView
{
    private readonly StatsStorageService _storage = new();

    public PageResultaten()
    {
        InitializeComponent();
        LoadStats();
    }

    private async void LoadStats()
    {
        var stats = await _storage.LoadAsync();

        StatsGraph.Drawable = new StatsGraphDrawable(stats);

        if (!stats.Any())
        {
            BestWpmLabel.Text = "Beste WPM ooit: --";
            SummaryLabel.Text = "Nog geen resultaten beschikbaar.";
            return;
        }

        double bestWpm = stats.Max(s => s.Wpm);
        BestWpmLabel.Text = $"Beste WPM ooit: {bestWpm:0}";

        var last = stats.Last();
        SummaryLabel.Text =
            $"Laatste sessie:\n" +
            $"WPM: {last.Wpm:0}\n" +
            $"Nauwkeurigheid: {last.Accuracy:0.0}%\n" +
            $"Tijd: {last.TotalTime:mm\\:ss}\n" +
            $"Fouten: {last.Mistakes}";
    }
}
