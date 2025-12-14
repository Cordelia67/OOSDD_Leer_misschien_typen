using Typotrainer.Core.Services;
using Typotrainer.Services;

namespace Typotrainer.Views;

public partial class PageResultaten : ContentView
{
    private readonly StatsStorageService _statsStorageService;

    public PageResultaten(StatsStorageService statsStorageService)
    {
        InitializeComponent();
        _statsStorageService = statsStorageService;
        LoadStats();
    }

    private async void LoadStats()
    {
        var stats = await _statsStorageService.LoadAsync();

        StatsGraph.Drawable = new StatsGraphDrawable(stats);


        if (stats.Any())
        {
            var last = stats.Last();
            SummaryLabel.Text =
                $"Laatste sessie:\n" +
                $"WPM: {last.Wpm:0}\n" +
                $"Nauwkeurigheid: {last.Accuracy:0.0}%\n" +
                $"Tijd: {last.TotalTime:mm\\:ss}\n" +
                $"Fouten: {last.Mistakes}";
        }
    }
}
