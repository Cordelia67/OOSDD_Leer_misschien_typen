using Microsoft.Maui.ApplicationModel;
using Typotrainer.Services;

namespace Typotrainer.Views;

public partial class PageInstellingen : ContentView
{
    public PageInstellingen()
    {
        InitializeComponent(); //laadt de ui//

        ColorBlindSwitch.IsToggled = AccessibilitySettings.IsColorBlindModeEnabled; //als de toggle knop wordt gedrukt//
        AccessibilitySettings.ColorBlindModeChanged += OnColorBlindModeChanged;

        UpdatePreview();
    }

    private void OnColorBlindSwitchToggled(object sender, ToggledEventArgs e)
    {
        AccessibilitySettings.IsColorBlindModeEnabled = e.Value;
    }

    private void OnColorBlindModeChanged(object? sender, bool isEnabled)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (ColorBlindSwitch.IsToggled != isEnabled)
            {
                ColorBlindSwitch.Toggled -= OnColorBlindSwitchToggled;
                ColorBlindSwitch.IsToggled = isEnabled;
                ColorBlindSwitch.Toggled += OnColorBlindSwitchToggled;
            }

            UpdatePreview();
        });
    }

    private void UpdatePreview() //refreshed de kleuren //
    {
        PreviewCorrectLabel.TextColor = AccessibilitySettings.GetCorrectFeedbackColor();
        PreviewIncorrectLabel.TextColor = AccessibilitySettings.GetIncorrectFeedbackColor();
        PreviewIncorrectLabel.TextDecorations = AccessibilitySettings.GetIncorrectTextDecoration();
        PreviewIncorrectLabel.FontAttributes = AccessibilitySettings.GetIncorrectFontAttributes();
    }

    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        if (args.NewHandler is null)
        {
            AccessibilitySettings.ColorBlindModeChanged -= OnColorBlindModeChanged;
        }

        base.OnHandlerChanging(args);
    }
}