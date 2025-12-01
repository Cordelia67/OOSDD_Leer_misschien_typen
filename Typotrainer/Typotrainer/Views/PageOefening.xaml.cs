using Typotrainer.ViewModels;

namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
    private readonly ExerciseViewModel _viewModel;

    public PageOefening(ExerciseViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Toon placeholder
        CorrectText.Text = "Klik op start oefening om te starten.";
    }

    private void InputEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Update colored output
        ColoredOutput.FormattedText = _viewModel.GetColoredText();
    }

    private async void Startknop_Clicked(object sender, EventArgs e)
    {
        // Start de oefening via ViewModel
        _viewModel.StartExercise();

        // Update UI
        InputEditor.Text = "";
        ColoredOutput.FormattedText = new FormattedString();

        // Focus op input
        InputEditor.IsVisible = true;
        await Task.Delay(50);
        InputEditor.Focus();
    }
}