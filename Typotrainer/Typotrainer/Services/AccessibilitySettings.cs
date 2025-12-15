using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;

namespace Typotrainer.Services;

public static class AccessibilitySettings
{
    private const string ColorBlindModeKey = "Accessibility.ColorBlindMode";
    private static readonly WeakEventManager ColorBlindModeChangedManager = new();
    private static bool _isColorBlindModeEnabled = Preferences.Get(ColorBlindModeKey, false);

    public static event EventHandler<bool> ColorBlindModeChanged
    {
        add => ColorBlindModeChangedManager.AddEventHandler(value);
        remove => ColorBlindModeChangedManager.RemoveEventHandler(value);
    }

    public static bool IsColorBlindModeEnabled
    {
        get => _isColorBlindModeEnabled;
        set
        {
            if (_isColorBlindModeEnabled == value)
            {
                return;
            }

            _isColorBlindModeEnabled = value;
            Preferences.Set(ColorBlindModeKey, value);
            ColorBlindModeChangedManager.HandleEvent(null, value, nameof(ColorBlindModeChanged));
        }
    }

    public static Color GetCorrectFeedbackColor() => //Kleur als het goed is//
        _isColorBlindModeEnabled ? Color.FromArgb("#0D7C66") : Colors.Green;

    public static Color GetIncorrectFeedbackColor() => //Kleur als het woord slecht is//
        _isColorBlindModeEnabled ? Color.FromArgb("#D84727") : Colors.Red;

    public static TextDecorations GetIncorrectTextDecoration() => //Onderlijn van het woord//
        _isColorBlindModeEnabled ? TextDecorations.Underline : TextDecorations.None;

    public static FontAttributes GetIncorrectFontAttributes() => //Dikt gedrukt//
        _isColorBlindModeEnabled ? FontAttributes.Bold : FontAttributes.None;
}