using Typotrainer.ViewModels;

namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
    private readonly ExerciseViewModel _viewModel;
    private bool _isExerciseActive = false;

    public PageOefening(ExerciseViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to ViewModel events
        _viewModel.ExerciseCompleted += OnExerciseCompleted;

        // Toon placeholder
        ShowInstructions();
    }

    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isExerciseActive || _viewModel == null)
            return;

        try
        {
            // Update colored output
            ColoredOutput.FormattedText = _viewModel.GetColoredText();

            // Update status
            UpdateStatus();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in TextChanged: {ex.Message}");
        }
    }

    private async void Startknop_Clicked(object sender, EventArgs e)
    {
        if (_viewModel == null)
        {
            await ShowAlert("Fout", "ViewModel is niet beschikbaar");
            return;
        }

        try
        {
            // Start de oefening via ViewModel
            _viewModel.StartExercise();
            _isExerciseActive = true;

            // Update UI
            InputEditor.Text = "";
            ColoredOutput.FormattedText = new FormattedString();
            InputEditor.IsEnabled = true;

            // Update button en status
            StartButton.Text = "Oefening bezig...";
            StartButton.IsEnabled = false;
            StartButton.BackgroundColor = Colors.Gray;
            StatusLabel.Text = "Bezig met typen";
            StatusLabel.TextColor = Colors.Green;

            // Focus op input - probeer meerdere keren voor betere compatibiliteit
            await Task.Delay(100);
            InputEditor.Focus();

            // Extra focus attempt voor desktop platforms
            await Task.Delay(50);
            InputEditor.Focus();
        }
        catch (Exception ex)
        {
            await ShowAlert("Fout", $"Kan oefening niet starten: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Error starting exercise: {ex}");
        }
    }

    private void ShowInstructions()
    {
        CorrectText.Text = "Klik op 'Start oefening' om te beginnen.";
        StatusLabel.Text = "Niet gestart";
        StatusLabel.TextColor = Colors.Gray;
    }

    private void UpdateStatus()
    {
        if (_viewModel == null || string.IsNullOrEmpty(_viewModel.CorrectSentence))
            return;

        if (_viewModel.TypedText.Length == 0)
        {
            StatusLabel.Text = "Begin met typen...";
            StatusLabel.TextColor = Colors.Orange;
        }
        else if (_viewModel.TypedText.Length < _viewModel.CorrectSentence.Length)
        {
            int percentage = (int)((double)_viewModel.TypedText.Length / _viewModel.CorrectSentence.Length * 100);
            StatusLabel.Text = $"{percentage}% compleet";
            StatusLabel.TextColor = Colors.Blue;
        }
        else if (_viewModel.TypedText.Length >= _viewModel.CorrectSentence.Length)
        {
            StatusLabel.Text = "Voltooid!";
            StatusLabel.TextColor = Colors.Green;

            // De ViewModel zal automatisch een nieuwe oefening starten
            // We hoeven hier alleen de button state te resetten
            StartButton.IsEnabled = false;
        }
    }

    private void OnExerciseCompleted(object? sender, EventArgs e)
    {
        // UI update wanneer oefening compleet is
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            StatusLabel.Text = "Voltooid! Nieuwe oefening wordt geladen...";
            StatusLabel.TextColor = Colors.Green;

            // Clear input voor nieuwe oefening
            await Task.Delay(500);
            InputEditor.Text = "";
            ColoredOutput.FormattedText = new FormattedString();

            // Reset status en focus
            StatusLabel.Text = "Bezig met typen";
            StatusLabel.TextColor = Colors.Blue;
            StartButton.IsEnabled = false;

            await Task.Delay(100);
            InputEditor.Focus();
        });
    }

    private async Task ShowAlert(string title, string message)
    {
        // Zoek de parent page
        var page = GetParentPage();
        if (page != null)
        {
            await page.DisplayAlert(title, message, "OK");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"{title}: {message}");
        }
    }

    private Page? GetParentPage()
    {
        Element? current = this.Parent;
        while (current != null)
        {
            if (current is Page page)
                return page;
            current = current.Parent;
        }
        return null;
    }
}