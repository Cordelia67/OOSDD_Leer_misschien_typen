using Typotrainer.Services;
namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
	private readonly TypingService _typingService;
	private readonly string correctZin = "De snelle bruine vos springt over de luie hond."; //testzin later vervangen door random zin uit tekst bestand

    public PageOefening()
	{
		InitializeComponent();
		_typingService = new TypingService();
    }

	private void InputEdittor_TextChanged(object sender, TextChangedEventArgs e)
	{
		string typed = InputEditor.Text;

		if (string.IsNullOrEmpty(typed))
			return;

		int index = typed.Length - 1;
        if (index >= correctZin.Length)
			return;

		char typedChar = typed[index];
		bool isCorrect = _typingService.IsCorrectLetter(correctZin, index, typedChar);

		InputEditor.TextColor = isCorrect ? Colors.Green : Colors.Red;

    }
	private void Button_Clicked(object sender, EventArgs e)
    {

    }
}